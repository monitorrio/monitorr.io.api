using System;
using System.Collections.Generic;
using AutoPoco.Engine;

namespace Web.Infrastructure.Migrations.AutoPoco
{
    public class RandomPhoneNumberSource : DatasourceBase<string>
    {
        public override string Next(IGenerationContext context)
        {
            var dict = new Dictionary<int, string>
                           {
                               { 1, "139-555-0163" }, 
                               { 2, "976-555-0182" }, 
                               { 3, "978-555-0121" },
                               { 4, "381-555-0158" }, 
                               { 5, "383-555-0160" }, 
                               { 6, "385-555-0110" }
                           };
            var keyList = new List<int>(dict.Keys);
            var rand = new Random();
            var randomKey = keyList[rand.Next(keyList.Count)];
            return dict[randomKey];
        }
    }
}
