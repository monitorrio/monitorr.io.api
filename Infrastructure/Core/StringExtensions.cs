using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
	public static class StringExtensions
	{
		public static string Join(this IEnumerable<string> strings, string separator) {
			return String.Join(separator, strings.ToArray());
		}

		public static string Fmt(this string format, params object[] args) {
			return String.Format(format, args);
		}
	}
}