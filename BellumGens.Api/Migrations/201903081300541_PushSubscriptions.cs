namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PushSubscriptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PushSubscriptions",
                c => new
                    {
                        p256dh = c.String(nullable: false, maxLength: 128),
                        auth = c.String(nullable: false, maxLength: 128),
                        endpoint = c.String(),
                        expirationTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.p256dh, t.auth });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PushSubscriptions");
        }
    }
}
