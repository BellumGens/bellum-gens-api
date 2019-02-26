namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamVisility : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CSGOTeams", "Visible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CSGOTeams", "Visible");
        }
    }
}
