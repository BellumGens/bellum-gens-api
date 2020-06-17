namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TournamentActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tournaments", "Active");
        }
    }
}
