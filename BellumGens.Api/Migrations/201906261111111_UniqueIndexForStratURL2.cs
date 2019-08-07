namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueIndexForStratURL2 : DbMigration
    {
        public override void Up()
		{
			CreateIndex("dbo.CSGOStrategies", "CustomUrl", unique: true);
		}
        
        public override void Down()
		{
			DropIndex("dbo.CSGOStrategies", new[] { "CustomUrl" });
		}
    }
}
