using System;
using System.Collections.Generic;

namespace Core
{
	public static class Sequence
	{
		/// <summary>
		/// Generate an infinite sequence applying the given function each time
		/// </summary>
		public static IEnumerable<T> GetEnumerable<T>(Func<int, T> generator)
		{
			var i = 0;
			while (true)
				yield return generator(i++);
		}

		public static IEnumerator<T> GetEnumerator<T>(Func<T> generator) {
			return GetEnumerator(i => generator());
		}

		public static IEnumerator<T> GetEnumerator<T>(Func<int, T> generator) {
			return GetEnumerable(generator).GetEnumerator();
		}
		public static T GetNext<T>(this IEnumerator<T> enumerator)
		{
			if (!enumerator.MoveNext())
				return default(T);
			return enumerator.Current;
		}
	}
}