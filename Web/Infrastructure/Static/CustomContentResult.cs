using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Web.Infrastructure.Static
{
    public static class CustomContentResult<T>
    {
        internal static ContentResult ContentResult(object results)
        {
            var serializer = new JavaScriptSerializer
            {
                MaxJsonLength = Int32.MaxValue,
                RecursionLimit = 100
            };

            return new ContentResult
            {
                Content = serializer.Serialize(results),
                ContentType = "application/json",
            };
        }
    }
}
