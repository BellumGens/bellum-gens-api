namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Availability : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAvailabilities",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Day = c.Int(nullable: false),
                        From = c.DateTimeOffset(nullable: false, precision: 7),
                        To = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.Day })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAvailabilities", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserAvailabilities", new[] { "UserId" });
            DropTable("dbo.UserAvailabilities");
        }
    }
}
