namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingTournamentId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TournamentApplications");
            AlterColumn("dbo.TournamentApplications", "Id", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AddPrimaryKey("dbo.TournamentApplications", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TournamentApplications");
            AlterColumn("dbo.TournamentApplications", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.TournamentApplications", "Id");
        }
    }
}
