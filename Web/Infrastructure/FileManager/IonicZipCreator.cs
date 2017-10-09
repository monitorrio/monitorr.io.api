using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Configuration;
using Ionic.Zip;
using Web.Infrastructure.Static;

namespace Web.Infrastructure.FileManager
{
    public static class IonicZipCreator
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
            string filePath = Path.Combine(baseDir, fileName);
            using (ZipFile zip = new ZipFile())
            {
                zip.AddFiles(files, AppDeployment.Instance.AppSetting("PackagesDirectory", "~/App_Data/Packages"));
                zip.Save(filePath);
            }

            return filePath;
        }

        public static string GenerateZipName(int count)
        {
            return count +
                    "-inbox-files-" +
                    AppTime.Now().ToString("MM_dd_yyyy", CultureInfo.InvariantCulture) + "_" +
                    ShortGuid.NewGuid() + ".zip";
        }
    }
}
