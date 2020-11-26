namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamMember2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CSGOTeams", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.CSGOTeams", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.CSGOTeams", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CSGOTeams", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.CSGOTeams", "ApplicationUser_Id");
            AddForeignKey("dbo.CSGOTeams", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
