using BellumGens.Api.Models;
using System.Web.Http;

namespace BellumGens.Api.Controllers
{
    public class PushController : ApiController
    {
		BellumGensDbContext _dbContext = new BellumGensDbContext();

		[HttpPost]
		[Route("Subscribe")]
		public IHttpActionResult Subscribe(PushSubscription sub)
		{
			if (ModelState.IsValid)
			{
				_dbContext.PushSubscriptions.Add(sub);

				try
				{
					_dbContext.SaveChanges();
				}
				catch
				{
					return BadRequest("Error");
				}
				return Ok(sub);
			}
			return BadRequest("Error");
		}
    }
}
