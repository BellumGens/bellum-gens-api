using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BellumGens.Api.Startup))]

namespace BellumGens.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
		{
			SteamInfo.Initialize("./steam.settings");
			ConfigureAuth(app);
		}
    }
}
