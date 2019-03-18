namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSearchVisibility : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SearchVisibile", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SearchVisibile");
        }
    }
}
