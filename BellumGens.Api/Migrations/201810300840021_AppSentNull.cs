namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppSentNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TeamApplications", "Sent", c => c.DateTimeOffset(precision: 7, defaultValueSql: "getutcdate()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeamApplications", "Sent", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
