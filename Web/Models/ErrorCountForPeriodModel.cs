using System;

namespace Web.Models
{
    public class ErrorCountForPeriodModel
    {
        public string PeriodName { get; set; }
        public int Count { get; set; }
        public DateTime PeriodDate { get; set; }
        public bool ShowTimeOnly { get; set; }
    }
}