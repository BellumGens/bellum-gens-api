namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingMatches : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TournamentCSGOMatches", name: "TournamentCSGOGroup_Id", newName: "GroupId");
            RenameColumn(table: "dbo.TournamentSC2Match", name: "TournamentSC2Group_Id", newName: "GroupId");
            RenameIndex(table: "dbo.TournamentCSGOMatches", name: "IX_TournamentCSGOGroup_Id", newName: "IX_GroupId");
            RenameIndex(table: "dbo.TournamentSC2Match", name: "IX_TournamentSC2Group_Id", newName: "IX_GroupId");
            AddColumn("dbo.CSGOMatchMaps", "TeamPickId", c => c.Guid());
            AddColumn("dbo.CSGOMatchMaps", "TeamBanId", c => c.Guid());
            AddColumn("dbo.SC2MatchMap", "PlayerPickId", c => c.String(maxLength: 128));
            AddColumn("dbo.SC2MatchMap", "PlayerBanId", c => c.String(maxLength: 128));
            CreateIndex("dbo.CSGOMatchMaps", "TeamPickId");
            CreateIndex("dbo.CSGOMatchMaps", "TeamBanId");
            CreateIndex("dbo.SC2MatchMap", "PlayerPickId");
            CreateIndex("dbo.SC2MatchMap", "PlayerBanId");
            AddForeignKey("dbo.CSGOMatchMaps", "TeamBanId", "dbo.CSGOTeams", "TeamId");
            AddForeignKey("dbo.CSGOMatchMaps", "TeamPickId", "dbo.CSGOTeams", "TeamId");
            AddForeignKey("dbo.SC2MatchMap", "PlayerBanId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.SC2MatchMap", "PlayerPickId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SC2MatchMap", "PlayerPickId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SC2MatchMap", "PlayerBanId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CSGOMatchMaps", "TeamPickId", "dbo.CSGOTeams");
            DropForeignKey("dbo.CSGOMatchMaps", "TeamBanId", "dbo.CSGOTeams");
            DropIndex("dbo.SC2MatchMap", new[] { "PlayerBanId" });
            DropIndex("dbo.SC2MatchMap", new[] { "PlayerPickId" });
            DropIndex("dbo.CSGOMatchMaps", new[] { "TeamBanId" });
            DropIndex("dbo.CSGOMatchMaps", new[] { "TeamPickId" });
            DropColumn("dbo.SC2MatchMap", "PlayerBanId");
            DropColumn("dbo.SC2MatchMap", "PlayerPickId");
            DropColumn("dbo.CSGOMatchMaps", "TeamBanId");
            DropColumn("dbo.CSGOMatchMaps", "TeamPickId");
            RenameIndex(table: "dbo.TournamentSC2Match", name: "IX_GroupId", newName: "IX_TournamentSC2Group_Id");
            RenameIndex(table: "dbo.TournamentCSGOMatches", name: "IX_GroupId", newName: "IX_TournamentCSGOGroup_Id");
            RenameColumn(table: "dbo.TournamentSC2Match", name: "GroupId", newName: "TournamentSC2Group_Id");
            RenameColumn(table: "dbo.TournamentCSGOMatches", name: "GroupId", newName: "TournamentCSGOGroup_Id");
        }
    }
}
