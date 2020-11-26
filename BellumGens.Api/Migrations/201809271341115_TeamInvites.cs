namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamInvites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamInvites",
                c => new
                    {
                        InvitingUserId = c.String(nullable: false, maxLength: 128),
                        InvitedUserId = c.String(nullable: false, maxLength: 128),
                        TeamId = c.Guid(nullable: false),
                        State = c.Int(nullable: false),
                        Sent = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.InvitingUserId, t.InvitedUserId, t.TeamId })
                .ForeignKey("dbo.AspNetUsers", t => t.InvitedUserId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.InvitingUserId, cascadeDelete: true)
                .ForeignKey("dbo.CSGOTeams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.InvitingUserId)
                .Index(t => t.InvitedUserId)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamInvites", "TeamId", "dbo.CSGOTeams");
            DropForeignKey("dbo.TeamInvites", "InvitingUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TeamInvites", "InvitedUserId", "dbo.AspNetUsers");
            DropIndex("dbo.TeamInvites", new[] { "TeamId" });
            DropIndex("dbo.TeamInvites", new[] { "InvitedUserId" });
            DropIndex("dbo.TeamInvites", new[] { "InvitingUserId" });
            DropTable("dbo.TeamInvites");
        }
    }
}
