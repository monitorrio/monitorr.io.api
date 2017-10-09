using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Web.Infrastructure.DataTables;

namespace Web.Infrastructure.Extensions
{
    public static class DataTableExtensions
    {
        public static bool HasInboxFilters(this List<DataTableKeyValuePair> data)
        {
            bool isDownloaded = Convert.ToBoolean(data.Where(n => n.name == "IsDownloaded").Select(x => x.value).FirstOrDefault());
            bool isNotDownloaded = Convert.ToBoolean(data.Where(n => n.name == "IsNotDownloaded").Select(x => x.value).FirstOrDefault());
            bool isDownloadedToBeDeleted = Convert.ToBoolean(data.Where(n => n.name == "IsDownloadedToBeDeleted").Select(x => x.value).FirstOrDefault());
            bool isNotDownloadedToBeDeleted = Convert.ToBoolean(data.Where(n => n.name == "IsNotDownloadedToBeDeleted").Select(x => x.value).FirstOrDefault());
            if (isNotDownloaded)
            {
                return true;
            }
            if (isDownloaded)
            {
                return true;
            }
            if (isDownloadedToBeDeleted)
            {
                return true;
            }
            if (isNotDownloadedToBeDeleted)
            {
                return true;
            }

            return false;
        }
        public static bool NeedsToBeExported(this List<DataTableKeyValuePair> data)
        {
            bool isCsv = Convert.ToBoolean(data.Where(n => n.name == "IsCsv").Select(x => x.value).FirstOrDefault());
            if (isCsv)
            {
                return true;
            }
            return false;
        }

        public static string ToLogId(this List<DataTableKeyValuePair> data)
        {
            return data.Where(n => n.name == "LogId").Select(x => x.value).FirstOrDefault();
        }
        public static List<DataTableKeyValuePair> ToKeyValuePair(this object query)
        {
            var jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Deserialize<List<DataTableKeyValuePair>>(query.ToString());
        }
    }
}
