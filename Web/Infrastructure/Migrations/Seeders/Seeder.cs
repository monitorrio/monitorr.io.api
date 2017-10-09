using Web.Infrastructure.Migrations.Seeders.Mongo;

namespace Web.Infrastructure.Migrations.Seeders
{
    public static class Seeder
    {
        public static void Initialize()
        {
            RoleSeeder.Seed();
            //LegacySeeder.SeedLogs();
        }
    }
}