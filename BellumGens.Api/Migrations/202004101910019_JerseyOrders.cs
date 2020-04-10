namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JerseyOrders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JerseyOrders",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        City = c.String(),
                        StreetAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JerseyDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Guid(nullable: false),
                        Cut = c.Int(nullable: false),
                        Size = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JerseyOrders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JerseyDetails", "OrderId", "dbo.JerseyOrders");
            DropIndex("dbo.JerseyDetails", new[] { "OrderId" });
            DropTable("dbo.JerseyDetails");
            DropTable("dbo.JerseyOrders");
        }
    }
}
