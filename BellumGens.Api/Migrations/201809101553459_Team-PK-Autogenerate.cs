namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamPKAutogenerate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TeamMembers", "TeamId", "dbo.CSGOTeams");
            DropIndex("dbo.TeamMembers", new[] { "TeamId" });
            DropPrimaryKey("dbo.TeamMembers");
            DropPrimaryKey("dbo.CSGOTeams");
            AlterColumn("dbo.TeamMembers", "TeamId", c => c.Guid(nullable: false));
            AlterColumn("dbo.CSGOTeams", "TeamId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AddPrimaryKey("dbo.TeamMembers", new[] { "TeamId", "UserId" });
            AddPrimaryKey("dbo.CSGOTeams", "TeamId");
            CreateIndex("dbo.TeamMembers", "TeamId");
            AddForeignKey("dbo.TeamMembers", "TeamId", "dbo.CSGOTeams", "TeamId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamMembers", "TeamId", "dbo.CSGOTeams");
            DropIndex("dbo.TeamMembers", new[] { "TeamId" });
            DropPrimaryKey("dbo.CSGOTeams");
            DropPrimaryKey("dbo.TeamMembers");
            AlterColumn("dbo.CSGOTeams", "TeamId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TeamMembers", "TeamId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.CSGOTeams", "TeamId");
            AddPrimaryKey("dbo.TeamMembers", new[] { "TeamId", "UserId" });
            CreateIndex("dbo.TeamMembers", "TeamId");
            AddForeignKey("dbo.TeamMembers", "TeamId", "dbo.CSGOTeams", "TeamId", cascadeDelete: true);
        }
    }
}
