namespace BellumGens.Api.Migrations
{
    using BellumGens.Api.Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public partial class UpdateStratAndTeamURLs : DbMigration
    {
        public override void Up()
        {
            var _dbContext = new BellumGensDbContext();
            var teams = _dbContext.Teams.ToList();
            foreach (CSGOTeam team in teams)
            {
                team.UniqueCustomUrl(_dbContext);
            }
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
