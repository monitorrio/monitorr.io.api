using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

namespace Core.Configuration
{
	/// <summary>
	/// Simple Facade over the ConfigurationManager to both add convenience methods (such as appsetting typing), and make things more testable
	/// </summary>
	public class AppDeployment
	{
		public static AppDeployment Instance { get; set; }

		/// <summary>
		/// Returns an application setting converted to the desired type. Or an optional default value if it does not exist.
		/// </summary>
		public T AppSetting<T>(string settingName, T ifNotSet = default(T)) {
			var x = appSetting<T>(settingName);
			return x.Item1 ? x.Item2 : ifNotSet;
		}

		public Func<string, string> ConnectionString = name => ConfigurationManager.ConnectionStrings[name].IfNotNull(c => c.ConnectionString);
		public Func<IEnumerable<CoreIntegrationsSection.Integration>> Modules = () => ((CoreIntegrationsSection)ConfigurationManager.GetSection("coreIntegrations")).Integrations;
		public CoreIntegrationsSection.Integration GetConfiguredModule(string moduleName) {
			moduleName = moduleName.Replace(" ", "");
			return Modules().FirstOrDefault(m => String.Equals(moduleName, m.Name.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase));
		}
		public CoreIntegrationsSection.Integration GetDefaultModule() {
			return Modules().FirstOrDefault(m => m.IsDefault) ?? Modules().FirstOrDefault();
		}

		/// <summary>
		/// Returns an application setting converted to the desired type. Throw an exception if it does not exist
		/// </summary>
		public T MandatoryAppSetting<T>(string settingName) {
			var x = appSetting<T>(settingName);
			if (x.Item1)
				return x.Item2;
			throw new ConfigurationErrorsException("Application setting '{0}' is mandatory. Please contact the system administrator to add it and restart the application.".Fmt(settingName));
		}

		/// <summary>
		/// Return all application settings that have been declared in the web.config appsettings or specifically set from the application.
		/// Useful for in-app debugging
		/// </summary>
		public IDictionary<string, dynamic> AllDeclaredSettings() {
			return ConfigurationManager.AppSettings.AllKeys
				.ToDictionary(k => k, k => ConfigurationManager.AppSettings[(string) k] as dynamic)
				.Merge(manuallySetSettings);
		}
		/// <summary>
		/// Manually set an application setting. This overrides settings froma nywhere else
		/// Useful for in-app debugging
		/// </summary>
		public void SetAppSetting(string name, string value) {
			manuallySetSettings[name] = value;
		}

		// [true,setting] if the appsetting exists, [false, default] otherwise
		static Tuple<bool, T> appSetting<T>(string settingName) {
			var s = manuallySetSettings.ContainsKey(settingName) ? manuallySetSettings[settingName] : ConfigurationManager.AppSettings[settingName];
			if (s == null)
				return Tuple.Create(false, default(T));
			return Tuple.Create(true, convert<T>(s));
		}

		static T convert<T>(dynamic s) {
			return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(s);
		}

		readonly static IDictionary<string, dynamic> manuallySetSettings = new Dictionary<string, dynamic>();
		static AppDeployment() {
			Instance = new AppDeployment();
		}

	}
}