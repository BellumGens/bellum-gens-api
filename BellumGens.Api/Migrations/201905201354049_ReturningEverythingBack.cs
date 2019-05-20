namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReturningEverythingBack : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CSGOTeams", new[] { "CustomUrl" });
            AlterColumn("dbo.CSGOTeams", "CustomUrl", c => c.String(maxLength: 64));
            CreateIndex("dbo.CSGOTeams", "CustomUrl", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.CSGOTeams", new[] { "CustomUrl" });
            AlterColumn("dbo.CSGOTeams", "CustomUrl", c => c.String(maxLength: 64));
        }
    }
}
