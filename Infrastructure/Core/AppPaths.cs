using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace Core
{
	public static class AppPaths
	{
		public class PathsInstance
		{
			public readonly DirectoryInfo LogsDirectory;
			public readonly DirectoryInfo TempDirectory;
			public readonly DirectoryInfo RootDirectory;

			public PathsInstance(DirectoryInfo rootDirectory, DirectoryInfo rootTempDirectory) {
	
				RootDirectory = rootDirectory;
				TempDirectory = rootTempDirectory;
				ensureExists(TempDirectory);

				LogsDirectory = relativeDir("logs");
				ensureExists(LogsDirectory);
			}


			DirectoryInfo relativeDir(params string[] dirNames) { return relativeDir(RootDirectory, dirNames); }
			static DirectoryInfo relativeDir(DirectoryInfo root, params string[] dirNames) { return relativeDir(root, dirNames.AsEnumerable()); }
			static DirectoryInfo relativeDir(DirectoryInfo root, IEnumerable<string> dirNames) {
				if (!dirNames.Any()) return root;
				var d = new DirectoryInfo(Path.Combine(root.FullName, dirNames.First()));
				ensureExists(d);
				return relativeDir(d, dirNames.Skip(1));
			}
			static void ensureExists(DirectoryInfo dir) {
				if (!dir.Exists) dir.Create();
			}

			/// <summary>
			/// Return a filepath to a temporary file in the temporary directory
			/// </summary>
			public string GetTempFilePath(string extension = "tmp") {
				return Path.Combine(TempDirectory.FullName, GetTempFileName(extension));
			}
			/// <summary>
			/// Return a temporary filename (not path) with the given extension
			/// </summary>
			public string GetTempFileName(string extension = "tmp") {
				//Note, I'm not sure this is the best way to generate temporary file names but 
				//`Path.GetTempFileName()` is definitely not as it creates an actual file which might
				//throw errors! Also will throw errors if more than 65K temp files exist;
				//`Guid.NewGuid()` also might be an option but is too long.
				return Path.ChangeExtension(Randomness.FileNames.GetNext(), extension);
			}
            /// <summary>
            /// Return Email template folder location
            /// </summary>
            public string GetEmailTemplatesFolder()
            {
                return Path.Combine(relativeDir().FullName, @"Infrastructure/EmailTemplates");
            }
            public string GetMigrationDataFolder()
            {
                return Path.Combine(relativeDir().FullName, @"Infrastructure/Migrations/Data");
            }
        }

		public static PathsInstance Instance { get; private set; }

		public static void SetCurrent(PathsInstance paths) { Instance = paths; }

		public static bool IsInitialized() {
			return Instance != null;
		}
	}
}