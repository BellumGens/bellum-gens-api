namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoShowField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TournamentCSGOMatches", "NoShow", c => c.Boolean(nullable: false));
            AddColumn("dbo.TournamentSC2Match", "NoShow", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TournamentSC2Match", "NoShow");
            DropColumn("dbo.TournamentCSGOMatches", "NoShow");
        }
    }
}
