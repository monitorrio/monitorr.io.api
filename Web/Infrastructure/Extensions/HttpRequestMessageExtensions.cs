using System.Net.Http;
using System.Web;

namespace Web.Infrastructure.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static string ToClientIp(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            return null;
        }
        public static string ToClientBrowser(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.Browser.Type;
            }
            return null;
        }
        public static string ToClientOs(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.Browser.Platform;
            }
            return null;
        }
    }
}
