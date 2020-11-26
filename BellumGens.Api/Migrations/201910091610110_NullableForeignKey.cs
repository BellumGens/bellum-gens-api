namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TournamentApplications", "TeamId", "dbo.CSGOTeams");
            DropIndex("dbo.TournamentApplications", new[] { "TeamId" });
            AlterColumn("dbo.TournamentApplications", "TeamId", c => c.Guid());
            CreateIndex("dbo.TournamentApplications", "TeamId");
            AddForeignKey("dbo.TournamentApplications", "TeamId", "dbo.CSGOTeams", "TeamId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TournamentApplications", "TeamId", "dbo.CSGOTeams");
            DropIndex("dbo.TournamentApplications", new[] { "TeamId" });
            AlterColumn("dbo.TournamentApplications", "TeamId", c => c.Guid(nullable: false));
            CreateIndex("dbo.TournamentApplications", "TeamId");
            AddForeignKey("dbo.TournamentApplications", "TeamId", "dbo.CSGOTeams", "TeamId", cascadeDelete: true);
        }
    }
}
