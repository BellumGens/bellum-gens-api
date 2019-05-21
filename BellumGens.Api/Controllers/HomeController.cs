using System.Web.Mvc;

namespace BellumGens.Api.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return Redirect(CORSConfig.allowedOrigins);
		}
	}
}
