using System;
using System.Globalization;
using System.Linq;

namespace Web.Infrastructure.Extensions {
	public static class DateTimeExtensions {

		/// <summary>
		/// Returns number of seconds since January 1st, 1970
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static long ToUnixTimestamp(this DateTime d){
			TimeSpan ts = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1));
			return (long)ts.TotalSeconds;
		}
		public static string ToRelativeString(this DateTime d, DateTime accordingTo){

			const int SECOND = 1;
			const int MINUTE = 60 * SECOND;
			const int HOUR = 60 * MINUTE;
			const int DAY = 24 * HOUR;
			const int MONTH = 30 * DAY;

			TimeSpan ts = new TimeSpan(accordingTo.Ticks - d.Ticks);
			double delta = Math.Abs(ts.TotalSeconds);

			if(delta < 0) {
				return "not yet";
			}

			if(delta < 1 * MINUTE) {
				return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
			}

			if(delta < 2 * MINUTE) {
				return "a minute ago";
			}

			if(delta < 45 * MINUTE) {
				return ts.Minutes + " minutes ago";
			}

			if(delta < 90 * MINUTE) {
				return "an hour ago";
			}

			if(delta < 24 * HOUR) {
				return ts.Hours + " hours ago";
			}

			if(delta < 48 * HOUR) {
				return "yesterday";
			}

			if(delta < 30 * DAY) {
				return ts.Days + " days ago";
			}

			if(delta < 12 * MONTH) {
				int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
				return months <= 1 ? "one month ago" : months + " months ago";
			}

			else {
				int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
				return years <= 1 ? "one year ago" : years + " years ago";
			}

		}
		public static DateTime FromUnixTimestamp(long ts) {
			DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return dtDateTime.AddSeconds(ts);
		}
        public static DateTime StartOfMonth()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
        }
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0);
        }
        public static DateTime EndOfMonth()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            return new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);
        }
        public static DateTime EndOfMonth(this DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            return new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);
        }
        public static DateTime StartOfDay()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            return new DateTime(year, month, day, 0, 0, 0);
        }
        public static DateTime StartOfDay(this DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            return new DateTime(year, month, day, 0, 0, 0);
        }
        public static DateTime EndOfDay()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            return new DateTime(year, month, day, 23, 59, 59);
        }
        public static DateTime EndOfDay(this DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            return new DateTime(year, month, day, 23, 59, 59);
        }
        public static string ToUrlString(this DateTime date)
        {
            return date.Month + "-" + date.Day + "-" + date.Year;
        }
        public static string FractionalHoursToString(decimal? hours, string format)
        {
            if (hours == null)
                return null;
            if (string.IsNullOrEmpty(format))
                format = "{0}:{1}";

            TimeSpan tspan = TimeSpan.FromHours((double)hours);
            if (tspan.Seconds > 30)
            { //round the seconds up for formatting
                tspan = tspan.Add(new TimeSpan(0, 1, 0));
            }

            return string.Format(format, (tspan.Days * 24) + tspan.Hours, (tspan.Minutes < 10) ? "0" + tspan.Minutes.ToString() : tspan.Minutes.ToString());
        }
        public static string FractionalHoursToString(decimal? hours)
        {

            return FractionalHoursToString(hours, null);
        }
        public static decimal? TimeStringToDecimal(string timeString)
        {
            decimal? time = -1;
            try
            {
                if (string.IsNullOrEmpty(timeString))
                {
                    time = null;
                }
                else if (!timeString.Contains(':'))
                {
                    time = Convert.ToDecimal(timeString);
                }
                else
                {
                    TimeSpan ts;

                    TimeSpan.TryParse(timeString, out ts);

                    time = (decimal)ts.TotalHours;
                }
            }
            catch (Exception ex)
            {

            }
            return time;

        }
        public static string DateFormat(string dtValue, string format)
        {
            if (dtValue == null) return null;
            format = format != "" ? format : "MM/dd/yy h:mm";
            dtValue = Convert.ToDateTime(dtValue.TrimEnd()).ToString(format);
            return dtValue;
        }
        public static string DateFormatToCalendar(this DateTime date)
        {
            return $"{date:M/d/yyyy}";
        }
        public static DateTime Parse(this string input)
        {
            string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt", 
                   "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss", 
                   "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt", 
                   "M/d/yyyy h:mm", "M/d/yyyy h:mm", 
                   "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};

            DateTime dateValue;

            DateTime.TryParseExact(input, formats, new CultureInfo("en-US"), DateTimeStyles.None, out dateValue);

            return dateValue;
        }
        public static string ToCalendar(this DateTime date)
        {
            return $"{date:MM/dd/yyyy}";
        }
        public static string ToFriendly(this DateTime date)
        {
            return $"{date:G}";
        }
    }
}
