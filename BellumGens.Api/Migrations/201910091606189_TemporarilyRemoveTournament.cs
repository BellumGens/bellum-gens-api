namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemporarilyRemoveTournament : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TournamentApplications", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.CSGOTeams", "Tournament_ID", "dbo.Tournaments");
            DropIndex("dbo.CSGOTeams", new[] { "Tournament_ID" });
            DropIndex("dbo.TournamentApplications", new[] { "TournamentId" });
            DropPrimaryKey("dbo.Tournaments");
            AlterColumn("dbo.Tournaments", "ID", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AddPrimaryKey("dbo.Tournaments", "ID");
            DropColumn("dbo.CSGOTeams", "Tournament_ID");
            DropColumn("dbo.TournamentApplications", "TournamentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TournamentApplications", "TournamentId", c => c.Guid(nullable: false));
            AddColumn("dbo.CSGOTeams", "Tournament_ID", c => c.Guid());
            DropPrimaryKey("dbo.Tournaments");
            AlterColumn("dbo.Tournaments", "ID", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Tournaments", "ID");
            CreateIndex("dbo.TournamentApplications", "TournamentId");
            CreateIndex("dbo.CSGOTeams", "Tournament_ID");
            AddForeignKey("dbo.CSGOTeams", "Tournament_ID", "dbo.Tournaments", "ID");
            AddForeignKey("dbo.TournamentApplications", "TournamentId", "dbo.Tournaments", "ID", cascadeDelete: true);
        }
    }
}
