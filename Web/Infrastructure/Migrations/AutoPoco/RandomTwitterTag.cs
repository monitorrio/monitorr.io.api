using System;
using System.Collections.Generic;
using AutoPoco.Engine;

namespace Web.Infrastructure.Migrations.AutoPoco
{
    public class RandomTwiterTag : DatasourceBase<string>
    {
        public override string Next(IGenerationContext context)
        {
            var dict = new Dictionary<int, string>
                           {
                               { 1, "@codinghorror" }, 
                               { 2, "@rexparker" }, 
                               { 3, "@kevinmontrose" },
                               { 4, "@BoltClock" }, 
                               { 5, "@spolsky" }, 
                               { 6, "@shanselman" }
                           };
            var keyList = new List<int>(dict.Keys);
            var rand = new Random();
            var randomKey = keyList[rand.Next(keyList.Count)];
            return dict[randomKey];
        }
    }
}
