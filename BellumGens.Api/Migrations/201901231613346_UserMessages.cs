namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserMessages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        From = c.String(maxLength: 128),
                        To = c.String(maxLength: 128),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "getutcdate()"),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.To)
                .ForeignKey("dbo.AspNetUsers", t => t.From)
                .Index(t => t.From)
                .Index(t => t.To);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMessages", "From", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserMessages", "To", "dbo.AspNetUsers");
            DropIndex("dbo.UserMessages", new[] { "To" });
            DropIndex("dbo.UserMessages", new[] { "From" });
            DropTable("dbo.UserMessages");
        }
    }
}
