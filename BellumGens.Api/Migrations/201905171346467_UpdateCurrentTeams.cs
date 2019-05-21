namespace BellumGens.Api.Migrations
{
	using BellumGens.Api.Models;
	using System;
    using System.Data.Entity.Migrations;
	using System.Linq;

	public partial class UpdateCurrentTeams : DbMigration
    {
        public override void Up()
        {
			var _dbContext = new BellumGensDbContext();
			var teams = _dbContext.Teams.ToList();
			foreach (CSGOTeam team in teams)
			{
				team.UniqueCustomUrl(_dbContext);
			}
			_dbContext.SaveChanges();
		}
        
        public override void Down()
        {
        }
    }
}
