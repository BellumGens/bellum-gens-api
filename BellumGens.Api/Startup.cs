using BellumGens.Api.Migrations;
using BellumGens.Api.Models;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

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

			// ApplyEntityUpdates();

			ConfigureAuth(app);
		}

		private void ApplyEntityUpdates()
		{
			using (BellumGensDbContext context = new BellumGensDbContext())
			{
				List<TournamentApplication> applications = context.TournamentApplications.Where(a => a.Game == Game.StarCraft2).ToList();

				foreach (TournamentApplication application in applications)
                {
					application.User.BattleNetId = application.BattleNetId;
                }

				try
				{
					context.SaveChanges();
				}
				catch (Exception e)
				{

				}
			}
		}
    }
}
