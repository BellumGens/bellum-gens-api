namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Companies : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TournamentApplications", "CompanyId", "dbo.Companies");
            DropIndex("dbo.TournamentApplications", new[] { "CompanyId" });
            DropPrimaryKey("dbo.Companies");
            AlterColumn("dbo.TournamentApplications", "CompanyId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Companies", "Name", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Companies", "Name");
            CreateIndex("dbo.Companies", "Name", unique: true);
            CreateIndex("dbo.TournamentApplications", "CompanyId");
            AddForeignKey("dbo.TournamentApplications", "CompanyId", "dbo.Companies", "Name");
            DropColumn("dbo.Companies", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "Id", c => c.Guid(nullable: false));
            DropForeignKey("dbo.TournamentApplications", "CompanyId", "dbo.Companies");
            DropIndex("dbo.TournamentApplications", new[] { "CompanyId" });
            DropIndex("dbo.Companies", new[] { "Name" });
            DropPrimaryKey("dbo.Companies");
            AlterColumn("dbo.Companies", "Name", c => c.String());
            AlterColumn("dbo.TournamentApplications", "CompanyId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Companies", "Id");
            CreateIndex("dbo.TournamentApplications", "CompanyId");
            AddForeignKey("dbo.TournamentApplications", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
