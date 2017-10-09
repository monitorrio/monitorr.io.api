using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AuthorizationContext = System.Web.Mvc.AuthorizationContext;

namespace Web.Infrastructure.Attributes
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _claimType;
        private readonly string _claimValue;

        public ClaimsAuthorizeAttribute(string type, string value)
        {
            _claimType = type;
            _claimValue = value;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            ClaimsPrincipal user = HttpContext.Current.User as ClaimsPrincipal;
            if (user == null)
            {
                HandleUnauthorizedRequests(filterContext);
                //HandleUnauthorizedRequest(filterContext);
            }
            else if (user.HasClaim(_claimType, _claimValue))
            {
                base.OnAuthorization(filterContext);
            }
            else if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                HandleUnauthorizedRequests(filterContext);
            }
            else
            {
                //HandleUnauthorizedRequest(filterContext);
                HandleUnauthorizedRequests(filterContext);
            }
        }

        public void HandleUnauthorizedRequests(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            var url = "Account/Login";
            filterContext.Result = new RedirectResult(url);
        }
    }
}