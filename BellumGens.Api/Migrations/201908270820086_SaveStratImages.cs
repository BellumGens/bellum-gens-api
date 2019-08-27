namespace BellumGens.Api.Migrations
{
    using BellumGens.Api.Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public partial class SaveStratImages : DbMigration
    {
        public override void Up()
        {
			var _dbContext = new BellumGensDbContext();
			var strats = _dbContext.Strategies.ToList();
			foreach (CSGOStrategy strat in strats)
			{
				strat.SaveStrategyImage();
			}
			_dbContext.SaveChanges();
		}
        
        public override void Down()
        {
        }
    }
}
