namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BattleTag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TournamentApplications", "BattleNetId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TournamentApplications", "BattleNetId");
        }
    }
}
