namespace BellumGens.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentUniqueId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.StrategyComments");
            AlterColumn("dbo.StrategyComments", "Id", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AddPrimaryKey("dbo.StrategyComments", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.StrategyComments");
            AlterColumn("dbo.StrategyComments", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.StrategyComments", "Id");
        }
    }
}
