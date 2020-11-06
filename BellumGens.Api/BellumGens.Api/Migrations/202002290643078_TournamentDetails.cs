namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TournamentDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "Description", c => c.String());
            AddColumn("dbo.Tournaments", "Logo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tournaments", "Logo");
            DropColumn("dbo.Tournaments", "Description");
        }
    }
}
