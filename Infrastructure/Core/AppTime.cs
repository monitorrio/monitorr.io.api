using System;

namespace Core
{
	public class AppTime
	{
		public static Func<DateTime> Now = () => DateTime.UtcNow;
	}
}