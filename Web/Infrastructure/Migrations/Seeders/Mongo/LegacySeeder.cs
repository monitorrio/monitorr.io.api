using System;
using System.IO;
using System.Linq;
using Core;
using Core.Domain;
using Web.Infrastructure.Repositories;

namespace Web.Infrastructure.Migrations.Seeders.Mongo
{
    public static class LegacySeeder
    {
        public static void SeedLogs()
        {
            GrowlHelper.SimpleGrowl("Seeding Legacy Logs");

            var lines = File.ReadAllLines(Path.Combine(AppPaths.Instance.GetMigrationDataFolder(), "legacy_logs.csv")).Select(a => a.Split(';'));

  
            var repo = new LogRepository();

            foreach (var l in lines)
            {
                var x = new Log();
                x.DateCreated = DateTime.Parse(l.First().Split(',')[0].Trimmed());
                x.DateModified = AppTime.Now();
                x.LogId = l.First().Split(',')[2].Trimmed();
                x.Name = l.First().Split(',')[3].Trimmed();
                x.UserId = l.First().Split(',')[4].Trimmed();
                x.ShortName = l.First().Split(',')[5].Trimmed();
                x.WidgetColor = l.First().Split(',')[6].Trimmed();

                var existing = repo.FindByLogId(x.LogId);
                if (!existing.Any())
                {
                    repo.Add(x);
                }
            }
        }
    }
}
