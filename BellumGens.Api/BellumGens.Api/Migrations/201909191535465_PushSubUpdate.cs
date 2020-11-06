namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PushSubUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BellumGensPushSubscriptions", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.BellumGensPushSubscriptions", new[] { "userId" });
            DropPrimaryKey("dbo.BellumGensPushSubscriptions");
            AlterColumn("dbo.BellumGensPushSubscriptions", "userId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.BellumGensPushSubscriptions", new[] { "p256dh", "auth" });
            CreateIndex("dbo.BellumGensPushSubscriptions", "userId");
            AddForeignKey("dbo.BellumGensPushSubscriptions", "userId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BellumGensPushSubscriptions", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.BellumGensPushSubscriptions", new[] { "userId" });
            DropPrimaryKey("dbo.BellumGensPushSubscriptions");
            AlterColumn("dbo.BellumGensPushSubscriptions", "userId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.BellumGensPushSubscriptions", new[] { "userId", "p256dh", "auth" });
            CreateIndex("dbo.BellumGensPushSubscriptions", "userId");
            AddForeignKey("dbo.BellumGensPushSubscriptions", "userId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
