namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestPending : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TournamentApplications", "CompanyId", "dbo.Companies");
            DropIndex("dbo.TournamentApplications", new[] { "CompanyId" });
            AlterColumn("dbo.TournamentApplications", "CompanyId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.TournamentApplications", "CompanyId");
            AddForeignKey("dbo.TournamentApplications", "CompanyId", "dbo.Companies", "Name", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TournamentApplications", "CompanyId", "dbo.Companies");
            DropIndex("dbo.TournamentApplications", new[] { "CompanyId" });
            AlterColumn("dbo.TournamentApplications", "CompanyId", c => c.String(maxLength: 128));
            CreateIndex("dbo.TournamentApplications", "CompanyId");
            AddForeignKey("dbo.TournamentApplications", "CompanyId", "dbo.Companies", "Name");
        }
    }
}
