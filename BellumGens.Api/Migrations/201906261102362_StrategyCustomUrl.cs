namespace BellumGens.Api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class StrategyCustomUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CSGOStrategies", "CustomUrl", c => c.String(maxLength: 64));
		}
        
        public override void Down()
        {
            DropColumn("dbo.CSGOStrategies", "CustomUrl");
        }
    }
}
