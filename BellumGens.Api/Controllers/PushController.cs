using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BellumGens.Api.Controllers
{
	[EnableCors(origins: CORSConfig.allowedOrigins, headers: CORSConfig.allowedHeaders, methods: CORSConfig.allowedMethods, SupportsCredentials = true)]
	[Authorize]
	[HostAuthentication(CookieAuthenticationDefaults.AuthenticationType)]
	[RoutePrefix("api/Push")]
	public class PushController : ApiController
    {
		BellumGensDbContext _dbContext = new BellumGensDbContext();
        private ApplicationUserManager _userManager;

        [HttpPost]
		[Route("Subscribe")]
		public IHttpActionResult Subscribe(BellumGensPushSubscriptionViewModel sub)
		{
			BellumGensPushSubscription push = new BellumGensPushSubscription()
			{
				endpoint = sub.endpoint,
				expirationTime = sub.expirationTime,
				userId = GetAuthUser().Id,
				p256dh = sub.keys.p256dh,
				auth = sub.keys.auth
			};
			_dbContext.PushSubscriptions.Add(push);

			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return Ok("Sub already exists...");
			}
			return Ok(push);
		}
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationUser GetAuthUser()
        {
            return UserManager.FindByName(User.Identity.GetUserName());
        }
    }
}
