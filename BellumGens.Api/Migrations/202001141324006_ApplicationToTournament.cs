namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationToTournament : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TournamentApplications", "TournamentId", c => c.Guid());
            CreateIndex("dbo.TournamentApplications", "TournamentId");
            AddForeignKey("dbo.TournamentApplications", "TournamentId", "dbo.Tournaments", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TournamentApplications", "TournamentId", "dbo.Tournaments");
            DropIndex("dbo.TournamentApplications", new[] { "TournamentId" });
            DropColumn("dbo.TournamentApplications", "TournamentId");
        }
    }
}
