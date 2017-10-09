using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SessionAuthenticationConfig), "PreAppStart")]

namespace Web
{
    public static class SessionAuthenticationConfig
    {
        public static void PreAppStart()
        {
            DynamicModuleUtility.RegisterModule(typeof(System.IdentityModel.Services.SessionAuthenticationModule));
        }
    }
}