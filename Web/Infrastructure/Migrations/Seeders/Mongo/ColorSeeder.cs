using System.IO;
using System.Linq;
using Core;
using Core.Domain;
using Web.Infrastructure.Repositories;
using File = System.IO.File;

namespace Web.Infrastructure.Migrations.Seeders.Mongo
{
    public static class ColorSeeder
    {
        public static void Seed()
        {
            //GrowlHelper.SimpleGrowl("Seeding Colors");
            var repo = new ColorRepository();

            repo.DeleteAll();


            var lines = File.ReadAllLines(Path.Combine(AppPaths.Instance.GetMigrationDataFolder(),"dbo_Colors.csv")).Select(a => a.Split(';'));

            foreach (var x in lines)
            {
                var c = new Color();
                c.Name = x.First().Split(',')[0].Trimmed();

                repo.Insert(c);
            }
        }
    }
}
