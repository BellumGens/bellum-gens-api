namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ESEA_Discord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ESEA", c => c.String());
            AddColumn("dbo.CSGOTeams", "Discord", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CSGOTeams", "Discord");
            DropColumn("dbo.AspNetUsers", "ESEA");
        }
    }
}
