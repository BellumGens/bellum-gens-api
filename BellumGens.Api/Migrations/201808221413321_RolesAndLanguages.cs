namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RolesAndLanguages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserLanguages",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LanguageId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LanguageId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.LanguageId);
            
            AddColumn("dbo.AspNetUsers", "PreferredPrimaryRole", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "PreferredSecondaryRole", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLanguages", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.UserLanguages", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserLanguages", new[] { "LanguageId" });
            DropIndex("dbo.UserLanguages", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "PreferredSecondaryRole");
            DropColumn("dbo.AspNetUsers", "PreferredPrimaryRole");
            DropTable("dbo.UserLanguages");
            DropTable("dbo.Languages");
        }
    }
}
