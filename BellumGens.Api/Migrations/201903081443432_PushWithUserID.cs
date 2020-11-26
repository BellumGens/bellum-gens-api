namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PushWithUserID : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.PushSubscriptions");
            AddColumn("dbo.PushSubscriptions", "userId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.PushSubscriptions", "p256dh", c => c.String());
            AlterColumn("dbo.PushSubscriptions", "auth", c => c.String());
            AddPrimaryKey("dbo.PushSubscriptions", "userId");
            CreateIndex("dbo.PushSubscriptions", "userId");
            AddForeignKey("dbo.PushSubscriptions", "userId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PushSubscriptions", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.PushSubscriptions", new[] { "userId" });
            DropPrimaryKey("dbo.PushSubscriptions");
            AlterColumn("dbo.PushSubscriptions", "auth", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.PushSubscriptions", "p256dh", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.PushSubscriptions", "userId");
            AddPrimaryKey("dbo.PushSubscriptions", new[] { "p256dh", "auth" });
        }
    }
}
