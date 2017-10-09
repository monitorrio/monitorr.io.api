using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Web.Infrastructure.Static
{
    public enum eAuthenticationProvider
    {
        Db = 1,
        Google = 2,
        Microsoft = 3,
        Github = 4,
        Facebook = 5
    }

    public static class AuthProvider
    {
        public static IIdentity Identity()
        {
            return ClaimsPrincipal.Current.Identity;
        }
        public static bool IsDb()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity) ClaimsPrincipal.Current.Identity;
            return !claimsIdentity.Claims.Any(x => AvailableSocialProviders().Contains(x.Value));
        }

        public static eAuthenticationProvider CurrentProvider()
        {
            var claimsIdentity = (ClaimsIdentity) ClaimsPrincipal.Current.Identity;
            var name = claimsIdentity.Claims.Any(x => x.Type == "provider")
                ? claimsIdentity.Claims.First(x => x.Type == "provider").Value
                : null;
            if (name == null) return eAuthenticationProvider.Db;

            switch (name)
            {
                case "google-oauth2":
                    return eAuthenticationProvider.Google;
                case "windowslive":
                    return eAuthenticationProvider.Microsoft;
                case "github":
                    return eAuthenticationProvider.Github;
                case "facebook":
                    return eAuthenticationProvider.Facebook;
            }
            return eAuthenticationProvider.Db;
        }

        public static List<string> AvailableSocialProviders()
        {
            return new List<string> { "google-oauth2", "windowslive", "github", "facebook" };
        }
    }
}

