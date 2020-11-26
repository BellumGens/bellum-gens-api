namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationSentDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamApplications", "Sent", c => c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "getutcdate()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamApplications", "Sent");
        }
    }
}
