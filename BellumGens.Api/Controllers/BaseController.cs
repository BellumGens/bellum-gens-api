using BellumGens.Api.Models;
using BellumGens.Api.Models.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BellumGens.Api.Controllers
{
	[EnableCors(origins: CORSConfig.allowedOrigins, headers: CORSConfig.allowedHeaders, methods: CORSConfig.allowedMethods, SupportsCredentials = true)]
	[HostAuthentication(CookieAuthenticationDefaults.AuthenticationType)]
	public class BaseController : ApiController
    {
		private ApplicationUserManager _userManager;
		private ApplicationRoleManager _roleManager;
		protected readonly BellumGensDbContext _dbContext = new BellumGensDbContext();

        protected ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			set
			{
				_userManager = value;
			}
		}

		protected ApplicationRoleManager RoleManager
		{
			get
			{
				return _roleManager ?? Request.GetOwinContext().Get<ApplicationRoleManager>();
			}
		}

		protected ApplicationUser GetAuthUser()
		{
			return User.Identity.IsAuthenticated ? UserManager.FindById(User.Identity.GetResolvedUserId()) : null;
		}

		protected bool UserIsInRole(string role)
		{
			return UserManager.IsInRole(User.Identity.GetResolvedUserId(), role);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && _userManager != null)
			{
				_userManager.Dispose();
				_userManager = null;
			}

			if (disposing && _roleManager != null)
			{
				_roleManager.Dispose();
				_roleManager = null;
			}

            if (disposing)
            {
                _dbContext.Dispose();
            }

            base.Dispose(disposing);
		}
	}
}
