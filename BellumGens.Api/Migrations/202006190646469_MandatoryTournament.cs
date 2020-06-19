namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MandatoryTournament : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TournamentApplications", "TournamentId", "dbo.Tournaments");
            DropIndex("dbo.TournamentApplications", new[] { "TournamentId" });
            AlterColumn("dbo.TournamentApplications", "TournamentId", c => c.Guid(nullable: false));
            CreateIndex("dbo.TournamentApplications", "TournamentId");
            AddForeignKey("dbo.TournamentApplications", "TournamentId", "dbo.Tournaments", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TournamentApplications", "TournamentId", "dbo.Tournaments");
            DropIndex("dbo.TournamentApplications", new[] { "TournamentId" });
            AlterColumn("dbo.TournamentApplications", "TournamentId", c => c.Guid());
            CreateIndex("dbo.TournamentApplications", "TournamentId");
            AddForeignKey("dbo.TournamentApplications", "TournamentId", "dbo.Tournaments", "ID");
        }
    }
}
