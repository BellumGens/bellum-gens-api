namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BindingTournamentAndMatches : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TournamentCSGOMatches", "TournamentId", c => c.Guid());
            AddColumn("dbo.TournamentSC2Match", "TournamentId", c => c.Guid());
            CreateIndex("dbo.TournamentCSGOMatches", "TournamentId");
            CreateIndex("dbo.TournamentSC2Match", "TournamentId");
            AddForeignKey("dbo.TournamentCSGOMatches", "TournamentId", "dbo.Tournaments", "ID");
            AddForeignKey("dbo.TournamentSC2Match", "TournamentId", "dbo.Tournaments", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TournamentSC2Match", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentCSGOMatches", "TournamentId", "dbo.Tournaments");
            DropIndex("dbo.TournamentSC2Match", new[] { "TournamentId" });
            DropIndex("dbo.TournamentCSGOMatches", new[] { "TournamentId" });
            DropColumn("dbo.TournamentSC2Match", "TournamentId");
            DropColumn("dbo.TournamentCSGOMatches", "TournamentId");
        }
    }
}
