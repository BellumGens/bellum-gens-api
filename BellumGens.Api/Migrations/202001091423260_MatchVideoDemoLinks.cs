namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MatchVideoDemoLinks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TournamentCSGOMatches", "DemoLink", c => c.String());
            AddColumn("dbo.TournamentCSGOMatches", "VideoLink", c => c.String());
            AddColumn("dbo.TournamentSC2Match", "DemoLink", c => c.String());
            AddColumn("dbo.TournamentSC2Match", "VideoLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TournamentSC2Match", "VideoLink");
            DropColumn("dbo.TournamentSC2Match", "DemoLink");
            DropColumn("dbo.TournamentCSGOMatches", "VideoLink");
            DropColumn("dbo.TournamentCSGOMatches", "DemoLink");
        }
    }
}
