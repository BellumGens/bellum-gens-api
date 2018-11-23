namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMapToStrats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamStrategies", "Map", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamStrategies", "Map");
        }
    }
}
