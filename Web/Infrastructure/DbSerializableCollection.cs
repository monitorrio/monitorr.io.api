using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using Core;

namespace Web.Infrastructure
{
    /// <summary>
    /// Inherit from this to create a collection that will be serialized to Json in the db
    /// </summary>
    public abstract class DbSerializableCollection<T> : IEnumerable<T>
    {
        readonly ICollection<T> inner = new HashSet<T>();
        public void Set(params T[] objects)
        {
            inner.Clear();
            objects.ForEach(x => inner.Add(x));
        }

        public string Serialized
        {
            get { return JsonConvert.SerializeObject(inner); }
            set
            {
                Set(string.IsNullOrEmpty(value) ? new T[0] : JsonConvert.DeserializeObject<T[]>(value));
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return inner.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return inner.GetEnumerator();
        }
    }
}