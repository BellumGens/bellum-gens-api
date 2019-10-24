using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using BellumGens.Api.Results;
using Microsoft.Owin.Security.Cookies;
using System.Linq;

namespace BellumGens.Api.Controllers
{
	[Authorize]
	[RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {
        private const string LocalLoginProvider = "Local";
		private readonly BellumGensDbContext _dbContext = new BellumGensDbContext();

		private const string emailConfirmation = "Greetings,<br /><br />You have updated your account information on <a href='https://bellumgens.com' target='_blank'>bellumgens.com</a> with your email address.<br /><br />To confirm your email address click on this <a href='{0}' target='_blank'>link</a>.<br /><br />The Bellum Gens team<br /><br /><a href='https://bellumgens.com' target='_blank'>https://bellumgens.com</a>";

		public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
		[AllowAnonymous]
        [Route("UserInfo")]
        public async Task<UserStatsViewModel> GetUserInfo()
        {
			if (User.Identity.IsAuthenticated)
			{
                ApplicationUser user = GetAuthUser();
                UserStatsViewModel model = new UserStatsViewModel(user);
                if (string.IsNullOrEmpty(user.AvatarFull))
                {
                    model = await SteamServiceProvider.GetSteamUserDetails(user.Id);
                    model.SetUser(user);
                }
                model.externalLogins = UserManager.GetLogins(user.Id).Select(t => t.LoginProvider).ToList();
				return model;
			}
			return null;
        }

		[HttpPost]
		[AllowAnonymous]
		[Route("Subscribe")]
		public IHttpActionResult Subscribe(Subscriber sub)
		{
            if (ModelState.IsValid)
            {
                _dbContext.Subscribers.Add(sub);

                try
                {
                    _dbContext.SaveChanges();
                }
                catch
                {
                    return Ok("Already subscribed...");
                }
                return Ok("Subscribed successfully!");
            }
            return BadRequest("Email is not valid");
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("Unsubscribe")]
		public IHttpActionResult Unsubscribe(string email, Guid sub)
		{
			Subscriber subscriber = _dbContext.Subscribers.Find(email);
			if (subscriber?.SubKey == sub)
			{
				subscriber.Subscribed = false;
				try
				{
					_dbContext.SaveChanges();
				}
				catch
				{
					return BadRequest("Something went wrong");
				}
				return Redirect(CORSConfig.returnOrigin + "/emailconfirm/unsubscribed");
			}
			return BadRequest("Couldn't find the subscription...");
		}

		[Route("UserInfo")]
		[HttpPut]
		public async Task<IHttpActionResult> UpdateUserInfo(UserPreferencesViewModel preferences)
		{
            ApplicationUser user = GetAuthUser();
			bool newEmail = !string.IsNullOrEmpty(preferences.email) && preferences.email != user.Email && !user.EmailConfirmed;
			user.Email = preferences.email;
			user.SearchVisible = preferences.searchVisible;
			try
			{
				_dbContext.SaveChanges();
				if (newEmail)
				{
					string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
					var callbackUrl = Url.Link("ActionApi", new { controller = "Account", action = "ConfirmEmail", userId = user.Id, code });
					await UserManager.SendEmailAsync(user.Id, "Confirm your email", string.Format(emailConfirmation, callbackUrl));
				}
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok(new { newEmail, preferences.email });
		}

		[AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail")]
		public IHttpActionResult ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return Redirect(CORSConfig.returnOrigin + "/emailconfirm/error");
			}
			var result = UserManager.ConfirmEmail(userId, code);
			if (result.Succeeded)
			{
				return Redirect(CORSConfig.returnOrigin + "/emailconfirm");
			}
			return Redirect(CORSConfig.returnOrigin + "/emailconfirm/error");
		}

		// POST api/Account/Logout
		[Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

		// DELETE api/Account/Delete
		[HttpDelete]
		[Route("Delete")]
		public IHttpActionResult Delete(string userid)
		{
			if (GetAuthUser().Id != userid)
			{
				return BadRequest("User account mismatch...");
			}
			Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            ApplicationUser user = _dbContext.Users.Find(userid);
			_dbContext.Users.Remove(user);
			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok("Ok");
		}

		// GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
		//[Route("ManageInfo")]
		//public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
		//{
		//    IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

		//    if (user == null)
		//    {
		//        return null;
		//    }

		//    List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

		//    foreach (IdentityUserLogin linkedAccount in user.Logins)
		//    {
		//        logins.Add(new UserLoginInfoViewModel
		//        {
		//            LoginProvider = linkedAccount.LoginProvider,
		//            ProviderKey = linkedAccount.ProviderKey
		//        });
		//    }

		//    if (user.PasswordHash != null)
		//    {
		//        logins.Add(new UserLoginInfoViewModel
		//        {
		//            LoginProvider = LocalLoginProvider,
		//            ProviderKey = user.UserName,
		//        });
		//    }

		//    return new ManageInfoViewModel
		//    {
		//        LocalLoginProvider = LocalLoginProvider,
		//        Email = user.UserName,
		//        Logins = logins,
		//        ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
		//    };
		//}

		// POST api/Account/ChangePassword
		//[Route("ChangePassword")]
		//public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
		//{
		//    if (!ModelState.IsValid)
		//    {
		//        return BadRequest(ModelState);
		//    }

		//    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
		//        model.NewPassword);

		//    if (!result.Succeeded)
		//    {
		//        return GetErrorResult(result);
		//    }

		//    return Ok();
		//}

		// POST api/Account/SetPassword
		//[Route("SetPassword")]
		//public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
		//{
		//    if (!ModelState.IsValid)
		//    {
		//        return BadRequest(ModelState);
		//    }

		//    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

		//    if (!result.Succeeded)
		//    {
		//        return GetErrorResult(result);
		//    }

		//    return Ok();
		//}

		// POST api/Account/AddExternalLogin
		[Route("AddExternalLogin")]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [HttpGet]
		public async Task<IHttpActionResult> AddExternalLogin(string userId)
		{
			ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

			if (externalLogin == null)
			{
				return Redirect(CORSConfig.returnOrigin + "/players/" + userId);
			}

			IdentityResult result = await UserManager.AddLoginAsync(userId,
				new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

			if (!result.Succeeded)
			{
				return Redirect(CORSConfig.returnOrigin + "/unauthorized");
			}


            IEnumerable<Claim> claims = externalLogin.GetClaims();
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);
            Authentication.SignIn(identity);

            return Redirect(CORSConfig.returnOrigin + "/players/" + userId);
		}

		// POST api/Account/RemoveLogin
		[Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(GetAuthUser().Id);
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(GetAuthUser().Id,
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
		[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
		[AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null, string returnUrl = "")
        {
            Uri returnUri = new Uri(returnUrl.Length > 0 ? returnUrl : CORSConfig.returnOrigin);
            string returnHost = returnUri.GetLeftPart(UriPartial.Authority);
            string returnPath = returnUri.AbsolutePath;

            if (error != null)
            {
                return Redirect(returnHost + "/unauthorized");
			}

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return Redirect(returnHost + "/unauthorized");
			}

            if (externalLogin.LoginProvider != provider)
            {
				Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
				if (provider == "Steam")
				{
					return new ChallengeResult(provider, this);
				}
				else
				{
					return new ChallengeResult(provider, this, Url.Link("ActionApi", new { controller = "Account", action = "AddExternalLogin", userId = GetAuthUser().Id }));
				}
            }

			ApplicationUser user = GetAuthUser();

			bool hasRegistered = user != null;

            if (!hasRegistered)
            {
				if (externalLogin.LoginProvider == "Steam")
				{
					IdentityResult x = await Register(externalLogin);
					if (!x.Succeeded)
					{
						return Redirect(returnHost + "/unauthorized");
					}

                    if (!returnPath.StartsWith("/tournament-signup"))
                    {
                        user = GetAuthUser();
                        // Upon registration, redirect to the user's profile for information setup.
                        returnPath = "/players/" + user.Id + "/true";
                    }
				}
				else
				{
					return Redirect(returnHost + "/addsteam");
				}
			}
            IEnumerable<Claim> claims = externalLogin.GetClaims();
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);
            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            Authentication.SignIn(identity);

            return Redirect(returnHost + returnPath);

		}
		// GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
		[AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/RegisterExternal
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        //[Route("RegisterExternal")]
        //public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var info = await Authentication.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return InternalServerError();
        //    }

        //    var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

        //    IdentityResult result = await UserManager.CreateAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result); 
        //    }
        //    return Ok();
        //}

        #region Helpers

        private async Task<IdentityResult> Register(ExternalLoginData info)
		{
			string id = SteamServiceProvider.SteamUserId(info.ProviderKey);

			var user = new ApplicationUser() {
				Id = id,
				UserName = User.Identity.Name
			};
			user.InitializeDefaults();

			IdentityResult result = await UserManager.CreateAsync(user);
			if (!result.Succeeded)
			{
				return result;
			}
		
			return await UserManager.AddLoginAsync(user.Id, new UserLoginInfo(info.LoginProvider, info.ProviderKey));
		}

		private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider)
                };

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
