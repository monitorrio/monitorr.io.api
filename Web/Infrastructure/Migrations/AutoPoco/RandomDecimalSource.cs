using System;
using AutoPoco.Engine;

namespace Web.Infrastructure.Migrations.AutoPoco
{
    public class RandomDecimalSource : DatasourceBase<decimal>
    {
        private readonly decimal _min;
        private readonly decimal _max;
        private readonly Random _random = new Random();

        public RandomDecimalSource(decimal max)
            : this(0m, max)
        {
        }

        public RandomDecimalSource(decimal min, decimal max)
        {
            _min = min;
            _max = max;
        }

        public override decimal Next(IGenerationContext context)
        {
            var randomValue = _random.NextDouble();
            var stretched = (_max - _min) * (decimal)randomValue;

            return stretched - _min;
        }
    }
}
