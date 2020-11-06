namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamStrats : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamStrategies",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        TeamId = c.Guid(nullable: false),
                        Side = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CSGOTeams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamStrategies", "TeamId", "dbo.CSGOTeams");
            DropIndex("dbo.TeamStrategies", new[] { "TeamId" });
            DropTable("dbo.TeamStrategies");
        }
    }
}
