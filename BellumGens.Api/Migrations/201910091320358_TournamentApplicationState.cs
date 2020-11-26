namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TournamentApplicationState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TournamentApplications", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TournamentApplications", "State");
        }
    }
}
