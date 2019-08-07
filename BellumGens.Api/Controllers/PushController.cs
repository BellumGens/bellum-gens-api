using BellumGens.Api.Models;
using System.Web.Http;

namespace BellumGens.Api.Controllers
{
	[Authorize]
	[RoutePrefix("api/Push")]
	public class PushController : BaseController
    {
		BellumGensDbContext _dbContext = new BellumGensDbContext();

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
    }
}
