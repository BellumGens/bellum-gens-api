namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendingStrategies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StrategyComments",
                c => new
                    {
                        Id = c.Guid(nullable: false, defaultValueSql: "newsequentialid()"),
                        StratId = c.Guid(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Published = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "getutcdate()"),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TeamStrategies", t => t.StratId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.StratId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.StrategyVotes",
                c => new
                    {
                        StratId = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Vote = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StratId, t.UserId })
                .ForeignKey("dbo.TeamStrategies", t => t.StratId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.StratId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.TeamStrategies", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.TeamStrategies", "Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.TeamStrategies", "PrivateShareLink", c => c.String());
            CreateIndex("dbo.TeamStrategies", "UserId");
            AddForeignKey("dbo.TeamStrategies", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StrategyVotes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StrategyVotes", "StratId", "dbo.TeamStrategies");
            DropForeignKey("dbo.TeamStrategies", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StrategyComments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StrategyComments", "StratId", "dbo.TeamStrategies");
            DropIndex("dbo.StrategyVotes", new[] { "UserId" });
            DropIndex("dbo.StrategyVotes", new[] { "StratId" });
            DropIndex("dbo.StrategyComments", new[] { "UserId" });
            DropIndex("dbo.StrategyComments", new[] { "StratId" });
            DropIndex("dbo.TeamStrategies", new[] { "UserId" });
            DropColumn("dbo.TeamStrategies", "PrivateShareLink");
            DropColumn("dbo.TeamStrategies", "Visible");
            DropColumn("dbo.TeamStrategies", "UserId");
            DropTable("dbo.StrategyVotes");
            DropTable("dbo.StrategyComments");
        }
    }
}
