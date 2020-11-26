namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderProcessing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JerseyOrders", "Confirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.JerseyOrders", "Shipped", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JerseyOrders", "Shipped");
            DropColumn("dbo.JerseyOrders", "Confirmed");
        }
    }
}
