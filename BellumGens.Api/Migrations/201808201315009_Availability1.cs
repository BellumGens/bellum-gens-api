namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Availability1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAvailabilities", "Available", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAvailabilities", "Available");
        }
    }
}
