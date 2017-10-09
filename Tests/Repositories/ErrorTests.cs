using System;
using System.Collections.Generic;
using System.Diagnostics;
using Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Infrastructure.DataTables;
using Web.Infrastructure.Repositories;

namespace Tests.Repositories
{
    [TestClass]
    public class ErrorTests
    {
        public IErrorRepository ErrorRepository;

        public ErrorTests()
        {
            ErrorRepository = new ErrorRepository();
        }
        [TestMethod]
        public void Will_Find_Latest_By_Owner_Id()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var userId = "google-oauth2|111853395574096843271";
            var latest = ErrorRepository.FindLatestByUserId(userId);

            sw.Stop();
            Console.WriteLine("Seconds={0}", sw.Elapsed.Seconds);
            Console.WriteLine("Records={0}", latest.Count);
            Assert.IsTrue(sw.Elapsed.Seconds < 5);
        }

        [TestMethod]
        public void Will_Find_By_Query()
        {
            List<DataTableKeyValuePair> data = new List<DataTableKeyValuePair>
            {
                new DataTableKeyValuePair
                {
                    name = "logId",
                    value = "4ca9cd8c-5b4b-4de2-b37f-a8271d9394b7"
                }
            };

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var query = "2";
            IList<Error> items = ErrorRepository.Search(query, data);

            sw.Stop();

            string message = $"Seconds { sw.Elapsed.Seconds } Records { items.Count }";

            Assert.IsTrue(sw.Elapsed.Seconds < 3, message);
        }
    }
}
