namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamsUniqueSteamGroupId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CSGOTeams", "SteamGroupId", c => c.String(maxLength: 64));
            CreateIndex("dbo.CSGOTeams", "SteamGroupId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.CSGOTeams", new[] { "SteamGroupId" });
            AlterColumn("dbo.CSGOTeams", "SteamGroupId", c => c.String());
        }
    }
}
