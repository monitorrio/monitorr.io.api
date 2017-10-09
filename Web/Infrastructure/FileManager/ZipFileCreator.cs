using Core;
using System.IO;
using Core.Configuration;
using System.Globalization;
using System.IO.Compression;
using Web.Infrastructure.Static;
using System.Collections.Generic;

namespace Web.Infrastructure.FileManager
{
    public static class ZipFileCreator
    {
        /// <summary>
        /// Create a ZIP file of the files provided.
        /// </summary>
        /// <param name="fileName">The full path and name to store the ZIP file at.</param>
        /// <param name="files">The list of files to be added.</param>
        public static string CreateZipFile(string fileName, IEnumerable<string> files)
        {
            string baseDir = System.Web.HttpContext.Current.Server.MapPath(AppDeployment.Instance.AppSetting("PackagesDirectory", "~/App_Data/Packages"));

            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            string zipPath = Path.Combine(baseDir, fileName);

            // Create and open a new ZIP file
            var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);
            foreach (var file in files)
            {
                // Add the entry for each file
                zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
            }
            // Dispose of the object when we are done
            zip.Dispose();
            return zipPath;
        }

        public static string GenerateZipName(int count)
        {
            return  count + 
                    "-inbox-files-" +
                    AppTime.Now().ToString("MM_dd_yyyy", CultureInfo.InvariantCulture) + "_" + 
                    ShortGuid.NewGuid() + ".zip";
        }
    }
}
