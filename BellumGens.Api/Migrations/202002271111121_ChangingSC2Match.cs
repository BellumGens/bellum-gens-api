namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingSC2Match : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SC2MatchMap", "WinnerId", c => c.String());
            DropColumn("dbo.SC2MatchMap", "Winner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SC2MatchMap", "Winner", c => c.String());
            DropColumn("dbo.SC2MatchMap", "WinnerId");
        }
    }
}
