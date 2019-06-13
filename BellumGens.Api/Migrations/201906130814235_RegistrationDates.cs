namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistrationDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RegisteredOn", c => c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "getutcdate()"));
            AddColumn("dbo.AspNetUsers", "LastSeen", c => c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "getutcdate()"));
            AddColumn("dbo.CSGOTeams", "RegisteredOn", c => c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "getutcdate()"));
            AddColumn("dbo.CSGOStrategies", "LastUpdated", c => c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "getutcdate()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CSGOStrategies", "LastUpdated");
            DropColumn("dbo.CSGOTeams", "RegisteredOn");
            DropColumn("dbo.AspNetUsers", "LastSeen");
            DropColumn("dbo.AspNetUsers", "RegisteredOn");
        }
    }
}
