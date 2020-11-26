namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamMemberEditor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamMembers", "IsEditor", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamMembers", "IsEditor");
        }
    }
}
