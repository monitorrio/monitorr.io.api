using System.Security.Principal;
using System.Web;
namespace Web.Infrastructure.Static
{
    public enum eSecurityLevel
    {
        LogAccess = 0,
        Free = 1,
        Pro = 2,
        Enterprise = 3,
        Admin = 100
    }

    public static class SecurityLevel
    {
        public static bool LogAccessOnly()
        {
            IPrincipal user = HttpContext.Current.User;
            return user.IsInRole("LogAccess");
        }
        public static bool FreeOnly()
        {
            IPrincipal user = HttpContext.Current.User;
            return user.IsInRole("Free");
        }
        public static bool Pro()
        {
            var user = HttpContext.Current.User;
            return user.IsInRole("Pro");
        }
        public static bool Enterprise()
        {
            var user = HttpContext.Current.User;
            return user.IsInRole("Enterprise");
        }
        public static bool Admin()
        {
            var user = HttpContext.Current.User;
            return user.IsInRole("Admin");
        }


        public static bool ValidateSecurityLevel(eSecurityLevel securityLevel)
        {
            bool isAuthorized;
            switch (securityLevel)
            {
                case eSecurityLevel.Free:
                    isAuthorized = FreeOnly();
                    break;
                case eSecurityLevel.Pro:
                    isAuthorized = Pro();
                    break;
                case eSecurityLevel.Enterprise:
                    isAuthorized = Enterprise();
                    break;
                case eSecurityLevel.Admin:
                    isAuthorized = Admin();
                    break;
                default:
                    isAuthorized = false;
                    break;
            }
            return isAuthorized;
        }
        public static eSecurityLevel CurrentSecurityLevel()
        {
            IPrincipal user = HttpContext.Current.User;

            if (user.IsInRole("Admin") || ValidateSecurityLevel(eSecurityLevel.Admin))
            {
                return eSecurityLevel.Admin;
            }
            if (user.IsInRole("Enterprise") || ValidateSecurityLevel(eSecurityLevel.Enterprise))
            {
                return eSecurityLevel.Enterprise;
            }
            if (user.IsInRole("Pro") || ValidateSecurityLevel(eSecurityLevel.Pro))
            {
                return eSecurityLevel.Pro;
            }
            if (user.IsInRole("Free"))
            {
                return eSecurityLevel.Free;
            }
            return eSecurityLevel.Free;
        }
    }
}

