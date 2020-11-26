namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameStrategiesTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TeamStrategies", newName: "CSGOStrategies");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.CSGOStrategies", newName: "TeamStrategies");
        }
    }
}
