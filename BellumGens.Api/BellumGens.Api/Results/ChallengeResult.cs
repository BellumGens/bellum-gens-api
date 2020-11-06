using Microsoft.Owin.Security;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BellumGens.Api.Results
{
    public class ChallengeResult : IHttpActionResult
    {
        public ChallengeResult(string loginProvider, ApiController controller)
        {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

		public ChallengeResult(string loginProvider, ApiController controller, string redirectUri)
		{
			LoginProvider = loginProvider;
			Request = controller.Request;
			RedirectUri = redirectUri;
		}

		public string LoginProvider { get; set; }
        public HttpRequestMessage Request { get; set; }
		public string RedirectUri { get; set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
			if (RedirectUri != null)
			{
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				Request.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
			else
			{
				Request.GetOwinContext().Authentication.Challenge(LoginProvider);
			}

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
    }
}
