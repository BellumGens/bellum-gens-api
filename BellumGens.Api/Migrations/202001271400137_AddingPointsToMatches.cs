namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingPointsToMatches : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TournamentCSGOMatches", "Team1Points", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.TournamentCSGOMatches", "Team2Points", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.TournamentSC2Match", "Player1Points", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.TournamentSC2Match", "Player2Points", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TournamentSC2Match", "Player2Points");
            DropColumn("dbo.TournamentSC2Match", "Player1Points");
            DropColumn("dbo.TournamentCSGOMatches", "Team2Points");
            DropColumn("dbo.TournamentCSGOMatches", "Team1Points");
        }
    }
}
