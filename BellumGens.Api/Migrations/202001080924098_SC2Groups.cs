namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SC2Groups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TournamentSC2Group",
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
                "dbo.TournamentSC2Match",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Player1Id = c.String(maxLength: 128),
                        Player2Id = c.String(maxLength: 128),
                        StartTime = c.DateTimeOffset(nullable: false, precision: 7),
                        EndTime = c.DateTimeOffset(nullable: false, precision: 7),
                        TournamentSC2Group_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Player1Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Player2Id)
                .ForeignKey("dbo.TournamentSC2Group", t => t.TournamentSC2Group_Id)
                .Index(t => t.Player1Id)
                .Index(t => t.Player2Id)
                .Index(t => t.TournamentSC2Group_Id);
            
            CreateTable(
                "dbo.SC2MatchMap",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Map = c.Int(nullable: false),
                        SC2MatchId = c.Guid(nullable: false),
                        Winner = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TournamentSC2Match", t => t.SC2MatchId, cascadeDelete: true)
                .Index(t => t.SC2MatchId);
            
            AddColumn("dbo.TournamentApplications", "TournamentSC2GroupId", c => c.Guid());
            CreateIndex("dbo.TournamentApplications", "TournamentSC2GroupId");
            AddForeignKey("dbo.TournamentApplications", "TournamentSC2GroupId", "dbo.TournamentSC2Group", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TournamentSC2Group", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentApplications", "TournamentSC2GroupId", "dbo.TournamentSC2Group");
            DropForeignKey("dbo.TournamentSC2Match", "TournamentSC2Group_Id", "dbo.TournamentSC2Group");
            DropForeignKey("dbo.TournamentSC2Match", "Player2Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TournamentSC2Match", "Player1Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SC2MatchMap", "SC2MatchId", "dbo.TournamentSC2Match");
            DropIndex("dbo.SC2MatchMap", new[] { "SC2MatchId" });
            DropIndex("dbo.TournamentSC2Match", new[] { "TournamentSC2Group_Id" });
            DropIndex("dbo.TournamentSC2Match", new[] { "Player2Id" });
            DropIndex("dbo.TournamentSC2Match", new[] { "Player1Id" });
            DropIndex("dbo.TournamentSC2Group", new[] { "TournamentId" });
            DropIndex("dbo.TournamentApplications", new[] { "TournamentSC2GroupId" });
            DropColumn("dbo.TournamentApplications", "TournamentSC2GroupId");
            DropTable("dbo.SC2MatchMap");
            DropTable("dbo.TournamentSC2Match");
            DropTable("dbo.TournamentSC2Group");
        }
    }
}
