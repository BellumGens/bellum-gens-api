namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TournamentEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TournamentApplications",
                c => new
                    {
                        Id = c.Guid(nullable: false, defaultValueSql: "newsequentialid()"),
                        TournamentId = c.Guid(nullable: false),
                        TeamId = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        DateSubmitted = c.DateTimeOffset(nullable: false, precision: 7),
                        Game = c.Int(nullable: false),
                        Hash = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.CSGOTeams", t => t.TeamId, cascadeDelete: true)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.TournamentId)
                .Index(t => t.TeamId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Guid(nullable: false, defaultValueSql: "newsequentialid()"),
                        Name = c.String(),
                        Logo = c.String(),
                        Website = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        ID = c.Guid(nullable: false, defaultValueSql: "newsequentialid()"),
                        Name = c.String(),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.CSGOTeams", "Tournament_ID", c => c.Guid());
            CreateIndex("dbo.CSGOTeams", "Tournament_ID");
            AddForeignKey("dbo.CSGOTeams", "Tournament_ID", "dbo.Tournaments", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CSGOTeams", "Tournament_ID", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentApplications", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentApplications", "TeamId", "dbo.CSGOTeams");
            DropForeignKey("dbo.TournamentApplications", "CompanyId", "dbo.Companies");
            DropIndex("dbo.TournamentApplications", new[] { "CompanyId" });
            DropIndex("dbo.TournamentApplications", new[] { "TeamId" });
            DropIndex("dbo.TournamentApplications", new[] { "TournamentId" });
            DropIndex("dbo.CSGOTeams", new[] { "Tournament_ID" });
            DropColumn("dbo.CSGOTeams", "Tournament_ID");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Companies");
            DropTable("dbo.TournamentApplications");
        }
    }
}
