using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Infrastructure.Static
{
    public static class RandomGenerator
    {
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static int GenerateRandomNumber(int minValue, int maxValue)
        {
            if (maxValue <= minValue)
            {
                maxValue = (minValue + 1);
            }
            Random rnd = new Random();
            return rnd.Next(minValue,maxValue);
        }
    }
}
