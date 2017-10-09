using System;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using Core;
using Core.Domain;
using SharpAuth0;
using Web.Infrastructure.Repositories;
using Web.Infrastructure.Static;

namespace Web.Controllers.Api
{
    public class BaseApiController : ApiController
    {
        public Task<User> CurrentUserAsync => _currentUserAsync.Value;
        private readonly Lazy<Task<User>> _currentUserAsync;
        public readonly IIdentityGateway IdentityGateway;
        protected string CurrentUserId { get; set; }
        protected User CurrentUser { get; }
        public bool IsDbUser { get; set; }
        public IIdentity Identity;
        public Claim Auth0User;

        public BaseApiController(IIdentityGateway identityGateway)
        {
            IdentityGateway = identityGateway;
            Identity = AuthProvider.Identity();
            if (!Identity.IsAuthenticated) return;

            Auth0User = IdentityGateway.FindIdentiyClaims();
            if (Auth0User == null) return;
            UserRepository userRepository = new UserRepository();
            var userId = userRepository.GetIdByAuth0Id(Auth0User.UserId);
            CurrentUserId = userId;
            IsDbUser = false;
        }

        public void LogVerbose(Exception ex)
        {
            if (ex == null)
                return;
            this.LogDebug("----------------------ERROR----------------------------------------");
            this.LogDebug($"{ ControllerContext.Request.RequestUri.AbsoluteUri } {ex.Message }");
            if (ex.InnerException == null) return;
            this.LogInfo(ex.InnerException.Message);
            if (ex.InnerException.InnerException == null) return;
            this.LogInfo(ex.InnerException.InnerException.Message);
            this.LogDebug("----------------------END ERROR----------------------------------------");
            GrowlHelper.SimpleGrowl(ControllerContext.Request.RequestUri.AbsoluteUri, $"with message: { ex.InnerException.Message }");
        }


    }
}