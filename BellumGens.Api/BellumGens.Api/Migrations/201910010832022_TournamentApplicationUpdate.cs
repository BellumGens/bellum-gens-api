namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TournamentApplicationUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TournamentApplications", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.TournamentApplications", "Email", c => c.String());
            CreateIndex("dbo.TournamentApplications", "UserId");
            AddForeignKey("dbo.TournamentApplications", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TournamentApplications", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.TournamentApplications", new[] { "UserId" });
            DropColumn("dbo.TournamentApplications", "Email");
            DropColumn("dbo.TournamentApplications", "UserId");
        }
    }
}
