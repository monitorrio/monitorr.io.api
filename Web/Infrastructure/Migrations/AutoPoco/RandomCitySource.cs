using System;
using System.Collections.Generic;
using System.Linq;
using AutoPoco.Engine;

namespace Web.Infrastructure.Migrations.AutoPoco
{
    public class RandomCitySource : DatasourceBase<string>
    {
        public override string Next(IGenerationContext context)
        {
            var cities = new List<string>
            {
                "AARONSBURG",
                "ABBEVILLE",
                "ABBOT",
                "ABBOTSFORD",
                "ABBOTT",
                "ABBOTTSTOWN",
                "ABBYVILLE",
                "ABELL",
                "ABERCROMBIE",
                "ABERDEEN",
                "ABERDEEN PROVING GROUND",
                "ABERNANT"
            };
            cities.AddRange(
                new List<string>
                {
                    "Katy",
                    "Crowder",
                    "Telford",
                    "Oscoda",
                    "Radisson",
                    "Buffalo",
                    "Napa",
                    "Petersville",
                    "Ladysmith",
                    "Marshall",
                    "Neilton",
                    "Northwoods",
                    "Manokotak",
                    "Menlo",
                    "Camdenton",
                    "Sam Rayburn",
                    "Painted Hills",
                    "Stovall",
                    "Leisure Village",
                    "Belle",
                    "Taos Pueblo",
                    "Far Hills",
                    "Wolf Summit",
                    "Quail Creek",
                    "Westville",
                    "Sautee Nacoochee",
                    "Amanda Park",
                    "Houtzdale",
                    "Saylorville",
                    "Winter Harbor",
                    "Escondido",
                    "Port St.John",
                    "Markham",
                    "Bendena",
                    "Adjuntas",
                    "Valinda",
                    "Stagecoach",
                    "Willow Valley",
                    "Ireton",
                    "Enville",
                    "Beattystown",
                    "Pryorsburg",
                    "Soudan",
                    "Brigantine",
                    "Vonore",
                    "Morales Sanchez",
                    "Ninilchik",
                    "Weir",
                    "North Massapequa",
                    "Burr Ridge"
                }
                );

            var rand = new Random();
            return cities.OrderBy(x => rand.Next()).Take(1).ToString();
        }
    }
}