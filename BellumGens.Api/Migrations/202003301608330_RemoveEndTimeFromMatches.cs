namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEndTimeFromMatches : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TournamentCSGOMatches", "EndTime");
            DropColumn("dbo.TournamentSC2Match", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TournamentSC2Match", "EndTime", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.TournamentCSGOMatches", "EndTime", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
