namespace BellumGens.Api.Migrations
{
    using BellumGens.Api.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public partial class UniqueIndexForStratURL : DbMigration
    {
        public override void Up()
        {
			var _dbContext = new BellumGensDbContext();
			var strats = _dbContext.Strategies.ToList();
			foreach (CSGOStrategy strat in strats)
			{
				strat.UniqueCustomUrl(_dbContext);
			}
			_dbContext.SaveChanges();
        }
        
        public override void Down()
        {
        }
    }
}
