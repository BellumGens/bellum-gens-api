using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using System;
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

		[HttpPost]
		[Route("Subscribe")]
		public IHttpActionResult Subscribe(PushSubscriptionViewModel sub)
		{
			string userId = SteamServiceProvider.SteamUserId(User.Identity.GetUserId());
			PushSubscription push = _dbContext.PushSubscriptions.Find(userId);
			
			if (push == null)
			{
				push = new PushSubscription()
				{
					endpoint = sub.endpoint,
					expirationTime = sub.expirationTime,
					userId = userId,
					p256dh = sub.keys.p256dh,
					auth = sub.keys.auth
				};
				_dbContext.PushSubscriptions.Add(push);
			}

			try
			{
				_dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				return BadRequest("Error");
			}
			return Ok(sub);
		}
    }
}
