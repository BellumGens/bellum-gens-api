namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamAdmin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CSGOTeamApplicationUsers", "CSGOTeam_TeamId", "dbo.CSGOTeams");
            DropPrimaryKey("dbo.CSGOTeams");
            AddColumn("dbo.CSGOTeams", "SteamGroupId", c => c.String());
            AlterColumn("dbo.CSGOTeams", "TeamId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.CSGOTeams", "TeamId");
            AddForeignKey("dbo.CSGOTeamApplicationUsers", "CSGOTeam_TeamId", "dbo.CSGOTeams", "TeamId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CSGOTeamApplicationUsers", "CSGOTeam_TeamId", "dbo.CSGOTeams");
            DropPrimaryKey("dbo.CSGOTeams");
            AlterColumn("dbo.CSGOTeams", "TeamId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.CSGOTeams", "SteamGroupId");
            AddPrimaryKey("dbo.CSGOTeams", "TeamId");
            AddForeignKey("dbo.CSGOTeamApplicationUsers", "CSGOTeam_TeamId", "dbo.CSGOTeams", "TeamId", cascadeDelete: true);
        }
    }
}
