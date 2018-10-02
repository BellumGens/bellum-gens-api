namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultSentDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TeamInvites", "Sent", c => c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "getutcdate()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeamInvites", "Sent", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
