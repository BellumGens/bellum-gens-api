namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JerseyOrders", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.JerseyOrders", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.JerseyOrders", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.JerseyOrders", "PhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.JerseyOrders", "City", c => c.String(nullable: false));
            AlterColumn("dbo.JerseyOrders", "StreetAddress", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JerseyOrders", "StreetAddress", c => c.String());
            AlterColumn("dbo.JerseyOrders", "City", c => c.String());
            AlterColumn("dbo.JerseyOrders", "PhoneNumber", c => c.String());
            AlterColumn("dbo.JerseyOrders", "LastName", c => c.String());
            AlterColumn("dbo.JerseyOrders", "FirstName", c => c.String());
            AlterColumn("dbo.JerseyOrders", "Email", c => c.String());
        }
    }
}
