using Core;
using System;
namespace Web.Infrastructure.Extensions
{
    public static class AutopocoExtensions
    {
        public static DateTime AdjustUploadDate(this DateTime uploadDate)
        {
            if (uploadDate > AppTime.Now())
            {
                uploadDate = AppTime.Now();
            }
            return uploadDate;
        }
        public static string AddFileExtension(this string fileName)
        {
            return fileName + ".jpg";
        }
    }
}
