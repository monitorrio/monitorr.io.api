using System;
using System.Collections.Generic;
using AutoPoco.Engine;

namespace Web.Infrastructure.Migrations.AutoPoco
{
    public class RandomUserNameSource : DatasourceBase<string>
    {
        public override string Next(IGenerationContext context)
        {
        
            var dict = new Dictionary<int, string>
                           {
                               { 1, "laurvasile" }, 
                               { 2, "terranmd" }, 
                               { 3, "protagonist" },
                               { 4, "vomar" }, 
                               { 5, "asigur123" }, 
                               { 6, "lenutsa1978" },
                               { 7, "crabcake" },
                               { 8, "easybreathy" },
                               { 9, "gandy" },
                               { 10, "terribble_boss" },
                               { 11, "coffe_maker" }
                           };

            var keyList = new List<int>(dict.Keys);
            var rand = new Random();
            var randomKey = keyList[rand.Next(keyList.Count)];
            return dict[randomKey];
        }
    }
}
