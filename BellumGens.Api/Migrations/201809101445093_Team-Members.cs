namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamMembers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CSGOTeamApplicationUsers", "CSGOTeam_TeamId", "dbo.CSGOTeams");
            DropForeignKey("dbo.CSGOTeamApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.CSGOTeamApplicationUsers", new[] { "CSGOTeam_TeamId" });
            DropIndex("dbo.CSGOTeamApplicationUsers", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        TeamId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TeamId, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.CSGOTeams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.CSGOTeams", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.CSGOTeams", "ApplicationUser_Id");
            AddForeignKey("dbo.CSGOTeams", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.CSGOTeamApplicationUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CSGOTeamApplicationUsers",
                c => new
                    {
                        CSGOTeam_TeamId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CSGOTeam_TeamId, t.ApplicationUser_Id });
            
            DropForeignKey("dbo.CSGOTeams", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TeamMembers", "TeamId", "dbo.CSGOTeams");
            DropForeignKey("dbo.TeamMembers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.TeamMembers", new[] { "UserId" });
            DropIndex("dbo.TeamMembers", new[] { "TeamId" });
            DropIndex("dbo.CSGOTeams", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.CSGOTeams", "ApplicationUser_Id");
            DropTable("dbo.TeamMembers");
            CreateIndex("dbo.CSGOTeamApplicationUsers", "ApplicationUser_Id");
            CreateIndex("dbo.CSGOTeamApplicationUsers", "CSGOTeam_TeamId");
            AddForeignKey("dbo.CSGOTeamApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CSGOTeamApplicationUsers", "CSGOTeam_TeamId", "dbo.CSGOTeams", "TeamId", cascadeDelete: true);
        }
    }
}
