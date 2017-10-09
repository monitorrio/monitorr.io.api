using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Core
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return (ExpandoObject)expando;
        }

        public static IDictionary<string, object> ToDictionary(this object anonymousObject)
        {
            return new RouteValueDictionary(anonymousObject);
        }
        public static bool NullSafeEquals(this object x1, object x2)
        {
            return x1.IfNotNull(x => x.Equals(x2), (Object.ReferenceEquals(x1, null) && Object.ReferenceEquals(x2, null)));
        }
        public static void IfNotNullThen<T>(this T o, Action<T> action) where T : class
        {
            if (o != null)
                action.Invoke(o);
        }

        public static R IfNotNull<T, R>(this T o, Func<T, R> returnFunc)
        {
            return IfNotNull(o, returnFunc, default(R));
        }

        public static R IfNotNull<T, R>(this T o, Func<T, R> returnFunc, R otherwise)
        {
            return ReferenceEquals(null, o) ? otherwise : returnFunc(o);
        }

        public static T NotNullOrThrow<T, E>(this T o, E ex) where E : Exception
        {
            if (Object.ReferenceEquals(null, o))
                throw ex;
            return o;
        }

        public static bool IsNull(this object obj)
        {
            return ReferenceEquals(obj, null);
        }

        public static bool IsNotNull(this object obj)
        {
            return !ReferenceEquals(obj, null);
        }

        public static VALUE TryGetValue<KEY, VALUE>(this IDictionary<KEY, VALUE> dict, KEY key)
        {
            VALUE value;
            dict.TryGetValue(key, out value);
            return value;
        }

        public static VALUE GetOrCache<KEY, VALUE>(this IDictionary<KEY, VALUE> dict, KEY key, Func<VALUE> getVal)
        {
            return dict.ContainsKey(key) ? dict[key] : (dict[key] = getVal());
        }

        public static IDictionary<KEY, VALUE> Slice<KEY, VALUE>(this IDictionary<KEY, VALUE> dict, params KEY[] keys)
        {
            return dict.Where(kv => keys.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static T[] WrapInArray<T>(this T item)
        {
            return new[] { item };
        }

        public static IEnumerable<T1> Prepend<T1, T2>(this IEnumerable<T1> collection, T2 obj, params T2[] more) where T2 : T1
        {
            yield return obj;
            foreach (var x in more) yield return x;
            foreach (var x in collection) yield return x;
        }
        public static IEnumerable<T1> Append<T1, T2>(this IEnumerable<T1> collection, T2 obj, params T2[] more) where T2 : T1
        {
            foreach (var x in collection) yield return x;
            yield return obj;
            foreach (var x in more) yield return x;
        }

        public static IEnumerable<T> Compact<T>(this IEnumerable<T> collection)
        {
            return collection.Where(x => !Object.ReferenceEquals(x, null));
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> act)
        {
            foreach (var x in collection)
                act(x);
            return collection;
        }
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T, int> act)
        {
            var i = 0;
            foreach (var x in collection)
                act(x, i++);
            return collection;
        }
        public static int? FindIndex<T>(this IEnumerable<T> collection, Func<T, bool> isMatch)
        {
            return collection.Where(isMatch).Select((_, i) => i).FirstOrDefault();
        }

        public static IEnumerable<Tuple<T1, T2>> FullJoin<T1, T2>(this IEnumerable<T1> collectionOne, IEnumerable<T2> collectionTwo, Func<T1, T2, bool> test)
        {
            return FullJoin(collectionOne, collectionTwo, test, Tuple.Create);
        }
        public static IEnumerable<R> FullJoin<T1, T2, R>(this IEnumerable<T1> collectionOne, IEnumerable<T2> collectionTwo, Func<T1, T2, bool> test, Func<T1, T2, R> createResult)
        {
            var coll1 = collectionOne.ToList();
            var coll2 = collectionTwo.ToList();
            foreach (var x1 in coll1)
                yield return createResult(x1, coll2.FirstOrDefault(x2 => test(x1, x2)));
            foreach (var x2 in coll2)
                if (coll1.None(x1 => test(x1, x2)))
                    yield return createResult(default(T1), x2);
        }
        public static IEnumerable<T> Except<T>(this IEnumerable<T> collection, params T[] withoutThese)
        {
            return collection.Except(withoutThese.AsEnumerable());
        }
        public static bool None<T>(this IEnumerable<T> collection)
        {
            return !collection.Any();
        }
        public static bool None<T>(this IEnumerable<T> collection, Func<T, bool> test)
        {
            return !collection.Any(test);
        }

        public static IDictionary<K, V> Clone<K, V>(this IDictionary<K, V> original)
        {
            if (original.IsNull()) return null;
            return new Dictionary<K, V>(original);
        }

        public static IDictionary<K, V> Merge<K, V>(this IDictionary<K, V> original, params IDictionary<K, V>[] others)
        {
            var d = original.Clone();
            var allPairs = others.SelectMany(other => other);
            allPairs.ForEach(kv => d[kv.Key] = kv.Value);
            return d;
        }

        public static IEnumerable<string> GetAllMessages(this Exception ex)
        {
            if (ex == null) return Enumerable.Empty<string>();
            return GetAllMessages(ex.InnerException).Append(ex.Message);
        }

        public static IEnumerable<string> CapturedGroupValues(this Regex rx, string input)
        {
            return rx.Match(input).Groups.Cast<Group>().Select(g => g.Value);
        }

    }
    public static class AwaitEx
    {
        public static R DoNow<R>(this Task<R> doing)
        {
            doing.Wait();
            return doing.Result;
        }
        public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
        {
            return Task.Delay(timeSpan).GetAwaiter();
        }
        public static TaskAwaiter<T[]> GetAwaiter<T>(this IEnumerable<Task<T>> tasks)
        {
            return Task.WhenAll(tasks).GetAwaiter();
        }
    }

    public static class StringModifier
    {
        public static string Trimmed(this string input)
        {
            return input.Replace('"', ' ').Trim();
        }
    }
}