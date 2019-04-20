namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditorMetadata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamStrategies", "EditorMetadata", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamStrategies", "EditorMetadata");
        }
    }
}
