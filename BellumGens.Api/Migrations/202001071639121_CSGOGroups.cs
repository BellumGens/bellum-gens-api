namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CSGOGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TournamentCSGOGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Name = c.String(),
                        TournamentId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.TournamentId);
            
            CreateTable(
                "dbo.TournamentCSGOMatches",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Team1Id = c.Guid(nullable: false),
                        Team2Id = c.Guid(nullable: false),
                        StartTime = c.DateTimeOffset(nullable: false, precision: 7),
                        EndTime = c.DateTimeOffset(nullable: false, precision: 7),
                        TournamentCSGOGroup_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CSGOTeams", t => t.Team1Id, cascadeDelete: false)
                .ForeignKey("dbo.CSGOTeams", t => t.Team2Id, cascadeDelete: false)
                .ForeignKey("dbo.TournamentCSGOGroups", t => t.TournamentCSGOGroup_Id)
                .Index(t => t.Team1Id)
                .Index(t => t.Team2Id)
                .Index(t => t.TournamentCSGOGroup_Id);
            
            CreateTable(
                "dbo.CSGOMatchMaps",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Map = c.Int(nullable: false),
                        CSGOMatchId = c.Guid(nullable: false),
                        Team1Score = c.Int(nullable: false),
                        Team2Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TournamentCSGOMatches", t => t.CSGOMatchId, cascadeDelete: true)
                .Index(t => t.CSGOMatchId);
            
            AddColumn("dbo.TournamentApplications", "TournamentCSGOGroupId", c => c.Guid());
            CreateIndex("dbo.TournamentApplications", "TournamentCSGOGroupId");
            AddForeignKey("dbo.TournamentApplications", "TournamentCSGOGroupId", "dbo.TournamentCSGOGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TournamentCSGOGroups", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentApplications", "TournamentCSGOGroupId", "dbo.TournamentCSGOGroups");
            DropForeignKey("dbo.TournamentCSGOMatches", "TournamentCSGOGroup_Id", "dbo.TournamentCSGOGroups");
            DropForeignKey("dbo.TournamentCSGOMatches", "Team2Id", "dbo.CSGOTeams");
            DropForeignKey("dbo.TournamentCSGOMatches", "Team1Id", "dbo.CSGOTeams");
            DropForeignKey("dbo.CSGOMatchMaps", "CSGOMatchId", "dbo.TournamentCSGOMatches");
            DropIndex("dbo.CSGOMatchMaps", new[] { "CSGOMatchId" });
            DropIndex("dbo.TournamentCSGOMatches", new[] { "TournamentCSGOGroup_Id" });
            DropIndex("dbo.TournamentCSGOMatches", new[] { "Team2Id" });
            DropIndex("dbo.TournamentCSGOMatches", new[] { "Team1Id" });
            DropIndex("dbo.TournamentCSGOGroups", new[] { "TournamentId" });
            DropIndex("dbo.TournamentApplications", new[] { "TournamentCSGOGroupId" });
            DropColumn("dbo.TournamentApplications", "TournamentCSGOGroupId");
            DropTable("dbo.CSGOMatchMaps");
            DropTable("dbo.TournamentCSGOMatches");
            DropTable("dbo.TournamentCSGOGroups");
        }
    }
}
