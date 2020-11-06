namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSteamDecoupling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AvatarFull", c => c.String());
            AddColumn("dbo.AspNetUsers", "AvatarMedium", c => c.String());
            AddColumn("dbo.AspNetUsers", "AvatarIcon", c => c.String());
            AddColumn("dbo.AspNetUsers", "RealName", c => c.String());
            AddColumn("dbo.AspNetUsers", "CustomUrl", c => c.String());
            AddColumn("dbo.AspNetUsers", "Country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Country");
            DropColumn("dbo.AspNetUsers", "CustomUrl");
            DropColumn("dbo.AspNetUsers", "RealName");
            DropColumn("dbo.AspNetUsers", "AvatarIcon");
            DropColumn("dbo.AspNetUsers", "AvatarMedium");
            DropColumn("dbo.AspNetUsers", "AvatarFull");
        }
    }
}
