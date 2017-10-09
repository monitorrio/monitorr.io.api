using System;

namespace Web.Infrastructure
{
	/// <summary>
	/// A maybe monad that is a better way of denoting a variable that may or may not exist than a null 
	/// (which requires nullable types, typechecking and might have another meaning).
	/// MaybeValue has implicit conversions to the underlying value and to an enumeration of values
	/// </summary>
	public class MaybeValue<T>
	{
		public readonly T Value;
		public readonly bool HasValue;

		public static readonly MaybeValue<T> Failure = new MaybeValue<T>();

		public static implicit operator bool(MaybeValue<T> av) {
			return av.HasValue;
		}

		public static implicit operator T(MaybeValue<T> av) {
			if (!av.HasValue)
				throw new InvalidOperationException("No value was available.");
			return av.Value;
		}

		public static implicit operator MaybeValue<T>(T value) {
			return new MaybeValue<T>(value);
		}
		public static implicit operator T[](MaybeValue<T> av) {
			if (!av.HasValue)
				return new T[0];
			return new[] { av.Value };
		}

		MaybeValue() { HasValue = false; }
		public MaybeValue(T value) {
			Value = value;
			HasValue = true;
		}
	}
	public class MaybeValue
	{
		public static MaybeValue<T> Create<T>(T val) { return new MaybeValue<T>(val); }
	}
}