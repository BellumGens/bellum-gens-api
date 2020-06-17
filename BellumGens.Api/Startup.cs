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
				Tournament tournament = context.Tournaments.FirstOrDefault();
				if (tournament != null)
				{
					tournament.CSGOMatches = context.TournamentCSGOMatches.ToList();
					tournament.SC2Matches = context.TournamentSC2Matches.ToList();
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
