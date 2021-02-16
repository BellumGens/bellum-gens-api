using BellumGens.Api.Migrations;
using Microsoft.Owin;
using Owin;
using System.Data.Entity.Migrations;

[assembly: OwinStartup(typeof(BellumGens.Api.Startup))]
namespace BellumGens.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
		{
			var configuration = new Configuration();
			var migrator = new DbMigrator(configuration);
			migrator.Update();

			ConfigureAuth(app);
		}
    }
}
