namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JerseyOrders", "OrderDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JerseyOrders", "OrderDate");
        }
    }
}
