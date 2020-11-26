namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriberKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscribers", "SubKey", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscribers", "SubKey");
        }
    }
}
