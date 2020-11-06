namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MapPool : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserMapPools",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Map = c.Int(nullable: false),
                        IsPlayed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.Map })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMapPools", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserMapPools", new[] { "UserId" });
            DropTable("dbo.UserMapPools");
        }
    }
}
