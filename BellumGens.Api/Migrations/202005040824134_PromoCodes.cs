namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoCodes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Promoes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Code)
                .Index(t => t.Code, unique: true);
            
            AddColumn("dbo.JerseyOrders", "PromoCode", c => c.String(maxLength: 128));
            CreateIndex("dbo.JerseyOrders", "PromoCode");
            AddForeignKey("dbo.JerseyOrders", "PromoCode", "dbo.Promoes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JerseyOrders", "PromoCode", "dbo.Promoes");
            DropIndex("dbo.Promoes", new[] { "Code" });
            DropIndex("dbo.JerseyOrders", new[] { "PromoCode" });
            DropColumn("dbo.JerseyOrders", "PromoCode");
            DropTable("dbo.Promoes");
        }
    }
}
