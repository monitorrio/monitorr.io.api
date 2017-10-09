using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

namespace Core.Configuration
{
	public class CoreIntegrationsSection : ConfigurationSection
	{
		[ConfigurationProperty("Integrations", IsRequired = true)]
		[ConfigurationCollection(typeof(Integration))]
		public GenericConfigurationElementCollection<Integration> Integrations {
			get { return this["Integrations"] as GenericConfigurationElementCollection<Integration>; }
		}
	
		public class Integration : ConfigurationElement
		{
			[ConfigurationProperty("Name", IsRequired=true)]
			public string Name {
				get { return this["Name"] as string; }
				set { this["Name"] = value; }
			}

			[ConfigurationProperty("Uri", IsRequired=true)]
			public string Uri {
				get { return this["Uri"] as string; }
				set { this["Uri"] = value; }
			}

			[ConfigurationProperty("Default", DefaultValue = false)]
			public bool IsDefault {
				get { return (bool)(this["Default"]); }
				set { this["Default"] = value; }
			}

		}
	}

	// Thanks http://stackoverflow.com/a/3952184/5056
	public class GenericConfigurationElementCollection<T> : ConfigurationElementCollection, IEnumerable<T> where T : ConfigurationElement, new() {
		protected override ConfigurationElement CreateNewElement() {
			T newElement = new T();
			_elements.Add(newElement);
			return newElement;
		}

		protected override object GetElementKey(ConfigurationElement element) {
			return _elements.Find(e => e.Equals(element));
		}

		public new IEnumerator<T> GetEnumerator() {
			return _elements.GetEnumerator();
		}
		readonly List<T> _elements = new List<T>();
	}
}