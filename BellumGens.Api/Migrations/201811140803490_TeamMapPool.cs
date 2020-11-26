namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamMapPool : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamMapPools",
                c => new
                    {
                        TeamId = c.Guid(nullable: false),
                        Map = c.Int(nullable: false),
                        IsPlayed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.TeamId, t.Map })
                .ForeignKey("dbo.CSGOTeams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamMapPools", "TeamId", "dbo.CSGOTeams");
            DropIndex("dbo.TeamMapPools", new[] { "TeamId" });
            DropTable("dbo.TeamMapPools");
        }
    }
}
