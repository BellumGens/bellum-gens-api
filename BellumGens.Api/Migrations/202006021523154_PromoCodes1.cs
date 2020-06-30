namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoCodes1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promoes", "Expiration", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promoes", "Expiration");
        }
    }
}
