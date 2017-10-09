using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Domain;

namespace Web.Infrastructure.Extensions
{
    public static class FileExtenstions
    {
        public static string ToFileSize(this long source)
        {
            const int byteConversion = 1024;
            double bytes = Convert.ToDouble(source);

            if (bytes >= Math.Pow(byteConversion, 3)) //GB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 3), 2), " GB");
            }
            if (bytes >= Math.Pow(byteConversion, 2)) //MB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 2), 2), " MB");
            }
            else if (bytes >= byteConversion) //KB Range
            {
                return string.Concat(Math.Round(bytes / byteConversion, 2), " KB");
            }
            else //Bytes
            {
                return string.Concat(bytes, " Bytes");
            }
        }
        public static string ToServerPath(this string baseDir, User user)
        {
            string filePath = Path.Combine(
                baseDir,
                AppTime.Now().Year.ToString(),
                AppTime.Now().ToString("MMM", CultureInfo.InvariantCulture),
                AppTime.Now().ToString("MM_dd_yyyy", CultureInfo.InvariantCulture),
                user.Email
             );

            var outputDir = new DirectoryInfo(filePath);
            if (!Directory.Exists(outputDir.FullName))
            {
                Directory.CreateDirectory(outputDir.FullName);
            }
            return outputDir.FullName;
        }
    }
}
