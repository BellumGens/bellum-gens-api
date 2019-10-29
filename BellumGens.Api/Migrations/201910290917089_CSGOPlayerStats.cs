namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CSGOPlayerStats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "HeadshotPercentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "KillDeathRatio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "Accuracy", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "SteamPrivate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SteamPrivate");
            DropColumn("dbo.AspNetUsers", "Accuracy");
            DropColumn("dbo.AspNetUsers", "KillDeathRatio");
            DropColumn("dbo.AspNetUsers", "HeadshotPercentage");
        }
    }
}
