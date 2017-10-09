using System.Security.Principal;
using Web.Infrastructure.Static;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Domain;
using SharpAuth0;
using System;
using Web.Infrastructure.Repositories;

namespace Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public Task<User> CurrentUserAsync => _currentUserAsync.Value;
        private readonly Lazy<Task<User>> _currentUserAsync;
        public readonly IIdentityGateway IdentityGateway;
        protected string CurrentUserId { get; set; }
        protected User CurrentUser { get; }
        public bool IsDbUser { get; set; }
        public IIdentity Identity;
        public Claim Auth0User;
  

        public BaseController(IIdentityGateway identityGateway)
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
    }
}