using System.Collections.Generic;

namespace Web.Models
{
    public class LogOverviewModel
    {
        public int TotalErrors { get; set; }
        public int CriticalErrors { get; set; }
        public int UniqueErrors { get; set; }
        public int UsersAffected { get; set; }
        public int BrowsersAffected { get; set; }

        public bool IsChromeAffected { get; set; }
        public bool IsFirefoxAffected { get; set; }
        public bool IsSafariAffected { get; set; }
        public bool IsIeAffected { get; set; }
        public bool IsOperaAffected { get; set; }
        public IEnumerable<string> BrowsersAffectedNames { get; set; }
        public int WarningPercent { get; set; }
        public int ErrorPercentage { get; set; }
        public IEnumerable<RecentErrorModel> RecentErrors { get; set; }
        public IEnumerable<FrequentErrorModel> FrequentErrors { get; set; }
        public IEnumerable<FrequentUrlModel> FrequentUrls { get; set; }
    }
}