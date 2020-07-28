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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using BellumGens.Api.Models.Extensions;

namespace BellumGens.Api.Controllers
{
	[Authorize]
	[RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {
        private const string LocalLoginProvider = "Local";

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

        // GET api/Account/Username
        [AllowAnonymous]
        [Route("Username")]
        public IHttpActionResult GetUsername(string username)
        {
            return Ok(_dbContext.Users.Any(u => u.UserName == username));
        }

        // GET api/Account/UserInfo
        [AllowAnonymous]
        [Route("UserInfo")]
        public async Task<UserStatsViewModel> GetUserInfo()
        {
			if (User.Identity.IsAuthenticated)
			{
                string userId = User.Identity.GetResolvedUserId();

                ApplicationUser user = _dbContext.Users.Include(u => u.MemberOf).FirstOrDefault(e => e.Id == userId);
                if (user == null)
                {
                    Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
                    return null;
                }

                UserStatsViewModel model = new UserStatsViewModel(user, true);
                if (user.SteamID != null && string.IsNullOrEmpty(user.AvatarFull))
                {
                    model = await SteamServiceProvider.GetSteamUserDetails(user.SteamID).ConfigureAwait(false);
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
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceInformation("Email subscription exception: " + e.Message);
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
				catch (DbUpdateException e)
				{
                    System.Diagnostics.Trace.TraceError("Email sub unsubscribe error: " + e.Message);
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
					string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id).ConfigureAwait(false);
					var callbackUrl = Url.Link("ActionApi", new { controller = "Account", action = "ConfirmEmail", userId = user.Id, code });
					await UserManager.SendEmailAsync(user.Id, "Confirm your email", string.Format(emailConfirmation, callbackUrl)).ConfigureAwait(false);
				}
			}
			catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("UserInfo update error: " + e.Message);
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

        // POST api/Account/Login
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(LoginBindingModel login)
        {
            var user = await UserManager.FindAsync(login.UserName, login.Password).ConfigureAwait(false);
            if (user != null)
            {
                var identity = await UserManager.CreateIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType).ConfigureAwait(false);
                Authentication.SignIn(new AuthenticationProperties() { IsPersistent = login.RememberMe }, identity);

                UserStatsViewModel model = new UserStatsViewModel(user, true);
                if (string.IsNullOrEmpty(user.AvatarFull) && !Guid.TryParse(user.Id, out Guid newguid))
                {
                    model = await SteamServiceProvider.GetSteamUserDetails(user.Id).ConfigureAwait(false);
                    model.SetUser(user);
                }
                model.externalLogins = UserManager.GetLogins(user.Id).Select(t => t.LoginProvider).ToList();
                return Ok(model);
            }
            return BadRequest("Invalid username or password.");
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
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
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);

            List<BellumGensPushSubscription> subs = _dbContext.PushSubscriptions.Where(s => s.userId == userid).ToList();
            foreach (var sub in subs)
            {
                _dbContext.PushSubscriptions.Remove(sub);
            }
            List<TeamInvite> invites = _dbContext.TeamInvites.Where(i => i.InvitedUserId == userid || i.InvitingUserId == userid).ToList();
            foreach (var invite in invites)
            {
                _dbContext.TeamInvites.Remove(invite);
            }
            ApplicationUser user = _dbContext.Users.Find(userid);
			_dbContext.Users.Remove(user);

			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
                System.Diagnostics.Trace.TraceError("User account delete error: " + e.Message);
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

        //    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetResolvedUserId(), model.OldPassword, model.NewPassword);

        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    return Ok();
        //}

        // POST api/Account/SetPassword
        [AllowAnonymous]
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!User.Identity.IsAuthenticated)
            {
                IdentityResult register = await Register(model).ConfigureAwait(false);
                if (!register.Succeeded)
                {
                    return GetErrorResult(register);
                }

                var newUser = await UserManager.FindAsync(model.UserName, model.Password).ConfigureAwait(false);
                try
                {
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(newUser.Id).ConfigureAwait(false);
                    var callbackUrl = Url.Link("ActionApi", new { controller = "Account", action = "ConfirmEmail", userId = newUser.Id, code });
                    await UserManager.SendEmailAsync(newUser.Id, "Confirm your email", string.Format(emailConfirmation, callbackUrl)).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.TraceError("Email confirmation send exception: " + e.Message);
                }

                return Ok();
            }

            string id = User.Identity.GetResolvedUserId();

            IdentityResult result = await UserManager.AddPasswordAsync(id, model.Password).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var user = await UserManager.FindByIdAsync(id).ConfigureAwait(false);
            if (user.UserName != model.UserName)
                user.UserName = model.UserName;
            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                result = await UserManager.UpdateAsync(user).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    try
                    {
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id).ConfigureAwait(false);
                        var callbackUrl = Url.Link("ActionApi", new { controller = "Account", action = "ConfirmEmail", userId = user.Id, code });
                        await UserManager.SendEmailAsync(user.Id, "Confirm your email", string.Format(emailConfirmation, callbackUrl)).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Trace.TraceError("Email confirmation send exception: " + e.Message);
                    }
                }
            }

            return Ok();
        }

        // GET api/Account/AddExternalLogin
        [Route("AddExternalLoginCallback")]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [HttpGet]
		public async Task<IHttpActionResult> AddExternalLoginCallback(string userId, string returnUrl)
		{
            Uri returnUri = new Uri(!string.IsNullOrEmpty(returnUrl) ? returnUrl : CORSConfig.returnOrigin);
            string returnHost = returnUri.GetLeftPart(UriPartial.Authority);
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

			if (externalLogin == null)
			{
				return Redirect(returnHost + "/unauthorized");
			}

            if (externalLogin.LoginProvider == "Steam")
            {
                string steamId = SteamServiceProvider.SteamUserId(externalLogin.ProviderKey);
                ApplicationUser user = _dbContext.Users.FirstOrDefault(u => u.SteamID == steamId);
                if (user != null && user.Id != userId)
                {
                    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    return Redirect(returnHost + "/unauthorized/Steam account already associated with another user");
                }
                if (user == null)
                {
                    user = _dbContext.Users.Find(userId);
                }

                user.SteamID = steamId;

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.TraceError("Setting user steam id exception: " + e.Message);
                }
            }

			IdentityResult result = await UserManager.AddLoginAsync(userId,
				new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey)).ConfigureAwait(false);

			if (!result.Succeeded)
			{
				return Redirect(returnHost + "/unauthorized");
			}

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            return Redirect(returnUrl);
		}

        // GET api/Account/AddExternalLogin
        [Route("AddExternalLogin", Name = "AddExternalLogin")]
        [HttpGet]
        public IHttpActionResult AddExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, this, Url.Link("ActionApi", new { controller = "Account", action = "AddExternalLoginCallback", userId = User.Identity.GetUserId(), returnUrl = returnUrl })); ;
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
                result = await UserManager.RemovePasswordAsync(GetAuthUser().Id).ConfigureAwait(false);
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(GetAuthUser().Id,
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey)).ConfigureAwait(false);
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
            Uri returnUri = new Uri(!string.IsNullOrEmpty(returnUrl) ? returnUrl : CORSConfig.returnOrigin);
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

			ApplicationUser user = GetAuthUser();

			bool hasRegistered = user != null;

            if (!hasRegistered)
            {
				IdentityResult x = await Register(externalLogin).ConfigureAwait(false);
				if (!x.Succeeded)
				{
					return Redirect(returnHost + "/unauthorized/Something went wrong");
				}
                returnPath = "/register";
			}
            else if (!UserManager.HasPassword(user.Id))
            {
                returnPath = "/register";
            }

            IEnumerable<Claim> claims = externalLogin.GetClaims();
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);
            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            Authentication.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);

            return Redirect(returnHost + returnPath);
		}

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, string routeName = "ExternalLogin", bool generateState = false)
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
                    Url = Url.Route(routeName, new
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
			string id = info.LoginProvider == "Steam" ? SteamServiceProvider.SteamUserId(info.ProviderKey) : Guid.NewGuid().ToString();
            string steamId = info.LoginProvider == "Steam" ? id : null;
			var user = new ApplicationUser() {
				Id = id,
                UserName = User.Identity.Name,
                SteamID = steamId
			};

			IdentityResult result = await UserManager.CreateAsync(user).ConfigureAwait(false);
			if (!result.Succeeded)
			{
				return result;
			}
		
			return await UserManager.AddLoginAsync(user.Id, new UserLoginInfo(info.LoginProvider, info.ProviderKey)).ConfigureAwait(false);
		}

        private async Task<IdentityResult> Register(RegisterBindingModel info)
        {
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = info.UserName,
                Email = info.Email
            };

            IdentityResult result = await UserManager.CreateAsync(user).ConfigureAwait(false);
            if (result.Succeeded)
            {
                result = await UserManager.AddPasswordAsync(user.Id, info.Password).ConfigureAwait(false);
            }
            return result;
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
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", nameof(strengthInBits));
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
