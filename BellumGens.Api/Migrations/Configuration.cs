namespace BellumGens.Api.Migrations
{
    using BellumGens.Api.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BellumGensDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BellumGens.Api.Models.BellumGensDbContext";
        }

        protected override void Seed(BellumGensDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
