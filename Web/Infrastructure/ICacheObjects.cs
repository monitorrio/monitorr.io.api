using System;
using System.Web;
using System.Web.Caching;
using Core;

namespace Web.Infrastructure
{
	/// <summary>
	/// Interface for a simple object store which will persist stored objects
	/// for a certain time
	/// </summary>
	public interface ICacheObjects
	{
		MaybeValue<T> Get<T>(string key);
		void Set(string key, dynamic obj, TimeSpan duration);
	}

	/// <summary>
	/// An implementation of ICacheObjects using Http Cache
	/// </summary>
	public class HttpCache : ICacheObjects
	{
		public MaybeValue<T> Get<T>(string key) {
			var val = HttpContext.Current.Cache.Get(key);
			if (val == null) {
				this.LogDebug("Failed to retrieve value at {0}", key);
				return MaybeValue<T>.Failure;
			}
			this.LogDebug("Retrieved value at {0}", key);
			return MaybeValue.Create((T)val);
		}

		public void Set(string key, dynamic obj, TimeSpan duration) {
			this.LogDebug("Storing value at {0} for {1}", key, duration);
			HttpContext.Current.Cache.Add(key, obj, null, DateTime.MaxValue, duration, CacheItemPriority.Default, null);
		}
	}
}