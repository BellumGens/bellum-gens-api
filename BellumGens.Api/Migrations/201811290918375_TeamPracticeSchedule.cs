namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamPracticeSchedule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamAvailabilities",
                c => new
                    {
                        TeamId = c.Guid(nullable: false),
                        Day = c.Int(nullable: false),
                        Available = c.Boolean(nullable: false),
                        From = c.DateTimeOffset(nullable: false, precision: 7),
                        To = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.TeamId, t.Day })
                .ForeignKey("dbo.CSGOTeams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
            AddColumn("dbo.TeamInvites", "Message", c => c.String());
            AlterColumn("dbo.TeamInvites", "Sent", c => c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "getutcdate()"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamAvailabilities", "TeamId", "dbo.CSGOTeams");
            DropIndex("dbo.TeamAvailabilities", new[] { "TeamId" });
            AlterColumn("dbo.TeamInvites", "Sent", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.TeamInvites", "Message");
            DropTable("dbo.TeamAvailabilities");
        }
    }
}
