﻿using BellumGens.Api.Models;
using BellumGens.Api.Providers;
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

		protected ApplicationUser GetAuthUser()
		{
			return User.Identity.IsAuthenticated ? UserManager.FindById(SteamServiceProvider.SteamUserId(User.Identity.GetUserId())) : null;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && _userManager != null)
			{
				_userManager.Dispose();
				_userManager = null;
			}

            if (disposing)
            {
                _dbContext.Dispose();
            }

            base.Dispose(disposing);
		}
	}
}
