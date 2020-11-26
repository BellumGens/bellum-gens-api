namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Teams : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CSGOTeams",
                c => new
                    {
                        TeamId = c.String(nullable: false, maxLength: 128),
                        TeamName = c.String(),
                        TeamAvatar = c.String(),
                    })
                .PrimaryKey(t => t.TeamId);
            
            CreateTable(
                "dbo.CSGOTeamApplicationUsers",
                c => new
                    {
                        CSGOTeam_TeamId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CSGOTeam_TeamId, t.ApplicationUser_Id })
                .ForeignKey("dbo.CSGOTeams", t => t.CSGOTeam_TeamId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.CSGOTeam_TeamId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CSGOTeamApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CSGOTeamApplicationUsers", "CSGOTeam_TeamId", "dbo.CSGOTeams");
            DropIndex("dbo.CSGOTeamApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CSGOTeamApplicationUsers", new[] { "CSGOTeam_TeamId" });
            DropTable("dbo.CSGOTeamApplicationUsers");
            DropTable("dbo.CSGOTeams");
        }
    }
}
