namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamCustomUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CSGOTeams", "CustomUrl", c => c.String(maxLength: 64));
		}
        
        public override void Down()
        {
            DropIndex("dbo.CSGOTeams", new[] { "CustomUrl" });
            DropColumn("dbo.CSGOTeams", "CustomUrl");
        }
    }
}
