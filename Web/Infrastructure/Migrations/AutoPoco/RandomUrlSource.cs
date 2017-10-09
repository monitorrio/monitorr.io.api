using System;
using System.Collections.Generic;
using AutoPoco.Engine;

namespace Web.Infrastructure.Migrations.AutoPoco
{
    public class RandomUrlSource : DatasourceBase<string>
    {
        public override string Next(IGenerationContext context)
        {
            var dict = new Dictionary<int, string>
                           {
                               { 1, "http://www.pennycluse.com/" }, 
                               { 2, "http://danielnyc.com/daniel.html" }, 
                               { 3, "http://www.perseny.com/" },
                               { 4, "http://www.21club.com/web/onyc/21_club.jsp?c=21campaign&p=toprestaurants&cr=home" }, 
                               { 5, "http://www.le-bernardin.com/" }, 
                               { 6, "http://www.benoitny.com/" }
                           };
            var keyList = new List<int>(dict.Keys);
            var rand = new Random();
            var randomKey = keyList[rand.Next(keyList.Count)];
            return dict[randomKey];
        }
    }
}
