using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using BellumGens.Api.Core.Models;
using BellumGens.Api.Core.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BellumGens.Api.Core.Models.Extensions;
using Microsoft.EntityFrameworkCore;
using BellumGens.Api.Core;
using Microsoft.AspNetCore.Http.Extensions;

namespace BellumGens.Api.Controllers
{
	[Authorize]
    public class AccountController : BaseController
    {
        private const string LocalLoginProvider = "Local";
		private const string emailConfirmation = "Greetings,<br /><br />You have updated your account information on <a href='https://bellumgens.com' target='_blank'>bellumgens.com</a> with your email address.<br /><br />To confirm your email address click on this <a href='{0}' target='_blank'>link</a>.<br /><br />The Bellum Gens team<br /><br /><a href='https://bellumgens.com' target='_blank'>https://bellumgens.com</a>";
        private readonly ISteamService _steamService;

		public AccountController(ISteamService steamService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSender sender, BellumGensDbContext context)
            : base(userManager, roleManager, signInManager, sender, context)
        {
            _steamService = steamService;
        }

        // GET api/Account/Username
        [AllowAnonymous]
        [Route("Username")]
        public IActionResult GetUsername(string username)
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
                string userId = User.GetResolvedUserId();

                ApplicationUser user = _dbContext.Users.Include(u => u.MemberOf).FirstOrDefault(e => e.Id == userId);
                if (user == null)
                {
                    await _signInManager.SignOutAsync();
                    return null;
                }

                UserStatsViewModel model = new UserStatsViewModel(user, true);
                if (user.SteamID != null && string.IsNullOrEmpty(user.AvatarFull))
                {
                    model = await _steamService.GetSteamUserDetails(user.SteamID);
                    model.SetUser(user, _dbContext);
                }
                var logins = await _userManager.GetLoginsAsync(user);
                model.externalLogins = logins.Select(t => t.LoginProvider).ToList();
				return model;
			}
			return null;
        }

		[HttpPost]
		[AllowAnonymous]
		[Route("Subscribe")]
		public IActionResult Subscribe(Subscriber sub)
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
		public IActionResult Unsubscribe(string email, Guid sub)
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
		public async Task<IActionResult> UpdateUserInfo(UserPreferencesViewModel preferences)
		{
            ApplicationUser user = await GetAuthUser();
			bool newEmail = !string.IsNullOrEmpty(preferences.email) && preferences.email != user.Email && !user.EmailConfirmed;
			user.Email = preferences.email;
			user.SearchVisible = preferences.searchVisible;
			try
			{
				_dbContext.SaveChanges();
				if (newEmail)
				{
					string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					var callbackUrl = Url.Link("ActionApi", new { controller = "Account", action = "ConfirmEmail", userId = user.Id, code });
					await _sender.SendEmailAsync(user.Email, "Confirm your email", string.Format(emailConfirmation, callbackUrl));
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
		public async Task<IActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return Redirect(CORSConfig.returnOrigin + "/emailconfirm/error");
			}
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    return Redirect(CORSConfig.returnOrigin + "/emailconfirm");
                }
            }
			return Redirect(CORSConfig.returnOrigin + "/emailconfirm/error");
        }

        // POST api/Account/Login
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginBindingModel login)
        {
            var user = await _userManager.FindByLoginAsync(login.UserName, login.Password);
            if (user != null)
            {
                await _signInManager.SignInAsync(user, login.RememberMe);

                UserStatsViewModel model = new UserStatsViewModel(user, true);
                if (string.IsNullOrEmpty(user.AvatarFull) && !Guid.TryParse(user.Id, out Guid newguid))
                {
                    model = await _steamService.GetSteamUserDetails(user.Id);
                    model.SetUser(user, _dbContext);
                }
                var logins = await _userManager.GetLoginsAsync(user);
                model.externalLogins = logins.Select(t => t.LoginProvider).ToList();
                return Ok(model);
            }
            return BadRequest("Invalid username or password.");
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

		// DELETE api/Account/Delete
		[HttpDelete]
		[Route("Delete")]
		public async Task<IActionResult> Delete(string userid)
		{
            ApplicationUser user = await GetAuthUser();

            if (user.Id != userid)
			{
				return BadRequest("User account mismatch...");
			}
            await _signInManager.SignOutAsync();

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
        public async Task<IActionResult> SetPassword(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!User.Identity.IsAuthenticated)
            {
                IdentityResult register = await Register(model);
                if (!register.Succeeded)
                {
                    return GetErrorResult(register);
                }

                var newUser = await _userManager.FindByLoginAsync(model.UserName, model.Password);
                try
                {
                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var callbackUrl = Url.Link("ActionApi", new { controller = "Account", action = "ConfirmEmail", userId = newUser.Id, code });
                    await _sender.SendEmailAsync(model.Email, "Confirm your email", string.Format(emailConfirmation, callbackUrl));
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.TraceError("Email confirmation send exception: " + e.Message);
                }

                return Ok();
            }

            ApplicationUser user = await GetAuthUser();

            IdentityResult result = await _userManager.AddPasswordAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            if (user.UserName != model.UserName)
                user.UserName = model.UserName;
            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    try
                    {
                        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Link("ActionApi", new { controller = "Account", action = "ConfirmEmail", userId = user.Id, code });
                        await _sender.SendEmailAsync(user.Email, "Confirm your email", string.Format(emailConfirmation, callbackUrl));
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
        [HttpGet]
		public async Task<IActionResult> AddExternalLoginCallback(string userId, string error = null, string returnUrl = "")
		{
            Uri returnUri = new Uri(!string.IsNullOrEmpty(returnUrl) ? returnUrl : CORSConfig.returnOrigin);
            string returnHost = returnUri.GetLeftPart(UriPartial.Authority);
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (error != null)
            {
                return Redirect(returnHost + "/unauthorized");
            }

            if (externalLogin == null)
			{
				return Redirect(returnHost + "/unauthorized");
			}

            ApplicationUser user = await GetAuthUser();

            if (externalLogin.LoginProvider == "Steam")
            {
                string steamId = _steamService.SteamUserId(externalLogin.ProviderKey);
                ApplicationUser steamuser = _dbContext.Users.FirstOrDefault(u => u.SteamID == steamId);
                if (steamuser != null && steamuser.Id != userId)
                {
                    await _signInManager.SignOutAsync();
                    return Redirect(returnHost + "/unauthorized/Steam account already associated with another user");
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

			IdentityResult result = await _userManager.AddLoginAsync(user,
				new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey, externalLogin.LoginProvider));

			if (!result.Succeeded)
			{
				return Redirect(returnHost + "/unauthorized");
			}

            return Redirect(returnUrl);
		}

        // GET api/Account/AddExternalLogin
        [Route("AddExternalLogin", Name = "AddExternalLogin")]
        [HttpGet]
        public IActionResult AddExternalLogin(string provider, string returnUrl)
        {
            string link = Url.Link("ActionApi", new { controller = "Account", action = "AddExternalLoginCallback", userId = User.GetResolvedUserId(), returnUrl = returnUrl });
            return new ChallengeResult(provider, new AuthenticationProperties() { RedirectUri = link }); ;
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await _userManager.RemovePasswordAsync(await GetAuthUser());
            }
            else
            {
                result = await _userManager.RemoveLoginAsync(await GetAuthUser(), model.LoginProvider, model.ProviderKey);
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [AllowAnonymous]
        [Route("Twitch", Name = "ExternalCallback")]
        public async Task<IActionResult> ExternalCallback(string error = null, string returnUrl = "", string userId = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            string returnPath = "";
            if (error != null)
            {
                return Redirect(returnUrl + "/unauthorized");
            }
            ApplicationUser user;

            if (userId != null)
            {
                user = await _userManager.FindByIdAsync(userId);
                IdentityResult result = await _userManager.AddLoginAsync(user,
                    new UserLoginInfo(info.LoginProvider, info.ProviderKey, info.ProviderDisplayName));
                if (!result.Succeeded)
                {
                    return Redirect(returnUrl + "/unauthorized");
                }
                return Redirect(returnUrl);
            }

            user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                var result = await Register(info);
                if (!result.Succeeded)
                {
                    return Redirect(returnUrl + "/unauthorized");
                }
                returnPath = "/register";
            }
            else if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
            if (!signInResult.Succeeded)
            {
                return Redirect(returnUrl + "/unauthorized");
            }
            return Redirect(returnUrl + returnPath);
        }

        // GET api/Account/ExternalLogin
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IActionResult> GetExternalLogin(string provider, string error = null, string returnUrl = "")
        {
            Uri returnUri = new Uri(!string.IsNullOrEmpty(returnUrl) ? returnUrl : CORSConfig.returnOrigin);
            returnUrl = returnUri.GetLeftPart(UriPartial.Authority);

            if (error != null)
            {
                return Redirect(returnUrl + "/unauthorized");
			}

            ApplicationUser user = await GetAuthUser();
            string userId = null;

            if (user != null)
            {
                userId = user.Id;
            }

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, Url.Action("ExternalCallback", "Account", new { returnUrl }), userId);
            return Challenge(properties, provider);

   //         ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

   //         if (externalLogin == null)
   //         {
   //             return Redirect(returnHost + "/unauthorized");
			//}

			//ApplicationUser user = await GetAuthUser();

			//bool hasRegistered = user != null;

   //         if (!hasRegistered)
   //         {
			//	IdentityResult x = await Register(externalLogin);
			//	if (!x.Succeeded)
			//	{
			//		return Redirect(returnHost + "/unauthorized/Something went wrong");
			//	}
   //             returnPath = "/register";
			//}

   //         await _signInManager.SignOutAsync();
   //         await _signInManager.SignInAsync(user, true);

   //         return Redirect(returnHost + returnPath);
		}

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public async Task<IEnumerable<ExternalLoginViewModel>> GetExternalLogins(string returnUrl, string routeName = "ExternalLogin", bool generateState = false)
        {
            IEnumerable<AuthenticationScheme> descriptions = await _signInManager.GetExternalAuthenticationSchemesAsync();
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

            foreach (AuthenticationScheme description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.DisplayName,
                    Url = Url.RouteUrl(routeName, new
                    {
                        provider = description.Name,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(new Uri(Request.GetDisplayUrl()), returnUrl).AbsoluteUri,
                        state
                    }, Request.Scheme),
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

        private async Task<IdentityResult> Register(ExternalLoginInfo info)
		{
            ApplicationUser user = null;
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var username = info.Principal.FindFirstValue(ClaimTypes.Name);
            var providerId = info.ProviderKey;
            switch (info.LoginProvider)
            {
                case "Twitch":
                    user = new ApplicationUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = username,
                        Email = email,
                        EmailConfirmed = true,
                        TwitchId = providerId
                    };
                    break;
                case "Steam":
                    user = new ApplicationUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = username,
                        Email = email,
                        EmailConfirmed = true,
                        SteamID = _steamService.SteamUserId(providerId)
                    };
                    break;
                case "BattleNet":
                    user = new ApplicationUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = username,
                        Email = email,
                        EmailConfirmed = true,
                        SteamID = _steamService.SteamUserId(providerId)
                    };
                    break;
                default:
                    break;
            }

			IdentityResult result = await _userManager.CreateAsync(user);
			if (!result.Succeeded)
			{
				return result;
			}
		
			return await _userManager.AddLoginAsync(user, new UserLoginInfo(info.LoginProvider, info.ProviderKey, info.LoginProvider));
		}

        private async Task<IdentityResult> Register(RegisterBindingModel info)
        {
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = info.UserName,
                Email = info.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddPasswordAsync(user, info.Password);
            }
            return result;
        }

        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
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
                    UserName = identity.FindFirst(ClaimTypes.Name).Value
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
                return Base64UrlTextEncoder.Encode(data);
            }
        }

        #endregion
    }
}
