using System;
using System.Collections.Generic;
using System.IO;

namespace Core
{
	/// <summary>
	/// Use this instead of the framework's classes for generating random values to allow stubbing in unit tests.
	/// </summary>
	public class Randomness
	{
		public static IEnumerator<Guid> Guid = Sequence.GetEnumerator(System.Guid.NewGuid);
		public static IEnumerator<string> FileNames = Sequence.GetEnumerator(_ => Path.GetRandomFileName().Replace(".", String.Empty));

		public static Guid NewGuid() { return Randomness.Guid.GetNext(); }
	}
}