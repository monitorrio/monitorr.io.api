using System;

namespace Web.Models
{
    public class ChartTimePeriod
    {
        private readonly string TimeFormat = "t";
        private readonly string DateFormat = "d";
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TimeZoneOffset { get; private set; }
        public bool ShowTimeOnly { get; private set; }

        public DateTime PeriodDate
        {
            get
            {
                if (StartDate < EndDate)
                {
                    return new DateTime(StartDate.AddTicks(EndDate.Ticks).Ticks / 2);
                }
                return StartDate;
            }
        }

        public string PeriodName => PeriodDate.AddHours(TimeZoneOffset).ToString(ShowTimeOnly ? TimeFormat : DateFormat);
        
        public ChartTimePeriod(double timeZoneOffset, bool showTimeOnly)
        {
            TimeZoneOffset = timeZoneOffset;
            ShowTimeOnly = showTimeOnly;
        }
    }
}