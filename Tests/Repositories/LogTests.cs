using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Infrastructure.Repositories;

namespace Tests.Repositories
{
    [TestClass]
    public class LogTests
    {
        public IErrorRepository ErrorRepository;
        public ILogRepository LogRepository;
        public IUserRepository UserRepository;

        public LogTests()
        {
            ErrorRepository = new ErrorRepository();
            UserRepository = new UserRepository();
            LogRepository = new LogRepository();
        }
        [TestMethod]
        public void Will_Update_Log_Color()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var widgetColor = "jarviswidget-color-greenLight";
            var logId = "4ca9cd8c-5b4b-4de2-b37f-a8271d9394b7";

            var log =  LogRepository.FindByLogId(logId).First();
            var previousColor = log.WidgetColor;
            log.WidgetColor = widgetColor;

            LogRepository.Update(log);


            string message = $"Updated { log.Name } with color { log.WidgetColor }";

            Assert.AreNotEqual(log.WidgetColor,previousColor);
        }

        [TestMethod]
        public async Task Will_Assign_New_Owner()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var currentOwnerId = "google-oauth2|111853395574096843271";
            var newOwnerId = "google-oauth2|115566555419040238295";

            var oldOwnerlogCount = LogRepository.FindAllByUserId(currentOwnerId).Count;
            await LogRepository.ReassignLogsOwnerAsync(currentOwnerId,newOwnerId);

            sw.Stop();

            string message = $"New owner assigned in { sw.Elapsed.Seconds } seconds";

            Assert.IsTrue(LogRepository.FindAllByUserId(newOwnerId).Count == oldOwnerlogCount);
        }

        [TestMethod]
        public async Task Will_Confirm_User_Has_Logs()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var newOwnerId = "google-oauth2|115566555419040238295";
            var logs = await LogRepository.FindAllByUserIdAsync(newOwnerId);
            sw.Stop();

            string message = $"New owner assigned in { sw.Elapsed.Seconds } seconds";

            Assert.IsTrue(logs.Count > 0);
        }
    }
}
