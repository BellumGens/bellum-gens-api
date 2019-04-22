namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PushNotifications : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BellumGensPushSubscriptions", "userId", "dbo.AspNetUsers");
            DropPrimaryKey("dbo.BellumGensPushSubscriptions");
            AlterColumn("dbo.BellumGensPushSubscriptions", "p256dh", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.BellumGensPushSubscriptions", "auth", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.BellumGensPushSubscriptions", new[] { "userId", "p256dh", "auth" });
            AddForeignKey("dbo.BellumGensPushSubscriptions", "userId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BellumGensPushSubscriptions", "userId", "dbo.AspNetUsers");
            DropPrimaryKey("dbo.BellumGensPushSubscriptions");
            AlterColumn("dbo.BellumGensPushSubscriptions", "auth", c => c.String());
            AlterColumn("dbo.BellumGensPushSubscriptions", "p256dh", c => c.String());
            AddPrimaryKey("dbo.BellumGensPushSubscriptions", "userId");
            AddForeignKey("dbo.BellumGensPushSubscriptions", "userId", "dbo.AspNetUsers", "Id");
        }
    }
}
