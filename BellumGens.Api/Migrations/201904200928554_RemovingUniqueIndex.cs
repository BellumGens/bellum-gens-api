namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingUniqueIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CSGOTeams", new[] { "SteamGroupId" });
            Sql(@"
                CREATE UNIQUE NONCLUSTERED INDEX IX_UQ_SteamGroupId
                ON dbo.CSGOTeams(SteamGroupId) 
                WHERE SteamGroupId IS NOT NULL;");
        }
        
        public override void Down()
        {
            CreateIndex("dbo.CSGOTeams", "SteamGroupId", unique: true);
        }
    }
}
