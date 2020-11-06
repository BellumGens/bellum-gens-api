namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionalTeam : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CSGOStrategies", "TeamId", "dbo.CSGOTeams");
            DropIndex("dbo.CSGOStrategies", new[] { "TeamId" });
            AlterColumn("dbo.CSGOStrategies", "TeamId", c => c.Guid());
            CreateIndex("dbo.CSGOStrategies", "TeamId");
            AddForeignKey("dbo.CSGOStrategies", "TeamId", "dbo.CSGOTeams", "TeamId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CSGOStrategies", "TeamId", "dbo.CSGOTeams");
            DropIndex("dbo.CSGOStrategies", new[] { "TeamId" });
            AlterColumn("dbo.CSGOStrategies", "TeamId", c => c.Guid(nullable: false));
            CreateIndex("dbo.CSGOStrategies", "TeamId");
            AddForeignKey("dbo.CSGOStrategies", "TeamId", "dbo.CSGOTeams", "TeamId", cascadeDelete: true);
        }
    }
}
