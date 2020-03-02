namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BattleNetAndSteamIDs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BattleNetId", c => c.String());
            AddColumn("dbo.AspNetUsers", "SteamID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SteamID");
            DropColumn("dbo.AspNetUsers", "BattleNetId");
        }
    }
}
