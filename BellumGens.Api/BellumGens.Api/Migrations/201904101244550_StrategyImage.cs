namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StrategyImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamStrategies", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamStrategies", "Image");
        }
    }
}
