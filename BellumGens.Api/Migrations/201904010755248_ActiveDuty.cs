namespace BellumGens.Api.Migrations
{
	using BellumGens.Api.Models;
	using System;
    using System.Data.Entity.Migrations;
	using System.Linq;

	public partial class ActiveDuty : DbMigration
    {
        public override void Up()
        {
			var _dbContext = new BellumGensDbContext();
			var players = _dbContext.Users.ToList();
			foreach (ApplicationUser player in players)
			{
				player.MapPool.Add(new UserMapPool()
				{
					Map = CSGOMaps.Vertigo
				});
			}
			var teams = _dbContext.Teams.ToList();
			foreach (CSGOTeam team in teams)
			{
				team.MapPool.Add(new BellumGens.Api.Models.TeamMapPool()
				{
					Map = CSGOMaps.Vertigo,
					IsPlayed = true
				});
			}
			_dbContext.SaveChanges();
        }

        public override void Down()
        {
        }
    }
}
