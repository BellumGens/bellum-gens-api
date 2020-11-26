namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CSGOTeams", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CSGOTeams", "Description");
        }
    }
}
