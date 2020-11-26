namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamApplications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamApplications",
                c => new
                    {
                        ApplicantId = c.String(nullable: false, maxLength: 128),
                        TeamId = c.Guid(nullable: false),
                        Message = c.String(),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicantId, t.TeamId })
                .ForeignKey("dbo.CSGOTeams", t => t.TeamId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicantId, cascadeDelete: true)
                .Index(t => t.ApplicantId)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamApplications", "ApplicantId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TeamApplications", "TeamId", "dbo.CSGOTeams");
            DropIndex("dbo.TeamApplications", new[] { "TeamId" });
            DropIndex("dbo.TeamApplications", new[] { "ApplicantId" });
            DropTable("dbo.TeamApplications");
        }
    }
}
