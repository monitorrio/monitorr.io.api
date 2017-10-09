using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
            "LogDetails",
            "Log/{id}",
            new { controller = "Log", action = "Index", id= ""},
            new[] { typeof(Controllers.LogController).Namespace }
        );

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { typeof(Controllers.HomeController).Namespace }
                );
        }
    }
}