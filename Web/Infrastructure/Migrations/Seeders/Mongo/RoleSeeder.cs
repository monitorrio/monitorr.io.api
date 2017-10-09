using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using Core;
using Core.Configuration;
using Core.Domain;
using Web.Infrastructure.Extensions;
using Web.Infrastructure.Repositories;

namespace Web.Infrastructure.Migrations.Seeders.Mongo
{
    public static class RoleSeeder
    {
        public static void Seed()
        {
            GrowlHelper.SimpleGrowl("Seeding Roles");
            var repo = new RoleRepository();
            string[] items = Enum.GetNames(typeof(RoleName));

            foreach (var x in items)
            {
                var existing = repo.GetRoleAsync(x.ToEnum<RoleName>());
                if (existing.Result == null)
                {
                    var e = new Role
                    {
                        RoleName = x.ToEnum<RoleName>(),
                        ApplicationName = AppDeployment.Instance.AppSetting("ApplicationName", "ElmahBucket")
                    };
                    repo.Insert(e);
                }
            }
        }
    }
}
