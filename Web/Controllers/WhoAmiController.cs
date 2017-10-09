using System.Web.Http;
using System.Web.Http.Cors;
using Core;
using Core.Configuration;
using SharpAuth0;
using Web.Controllers.Api;
using Web.Infrastructure.Repositories;

namespace Web.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("whoami")]
    public class WhoAmiController : BaseApiController
    {
        private readonly ILogRepository _logRepository;
        private readonly IErrorRepository _errorRepository;

        public WhoAmiController(
            ILogRepository logRepository,
            IErrorRepository errorRepository,
             IIdentityGateway identityGateway
            ) : base(identityGateway)
        {
            _logRepository = logRepository;
            _errorRepository = errorRepository;
        }

        [HttpGet, AllowAnonymous,Route("")]
        public IHttpActionResult Get()
        {
            this.LogDebug("Logging Operational");
            var model = new
            {
                Status = "Battlecruiser Operational",
                MongoUri = AppDeployment.Instance.AppSetting("MongoUri", ""),
                HangFireDatabase = AppDeployment.Instance.AppSetting("hangfire:DatabaseName", ""),
                Auth0ClientId = AppDeployment.Instance.AppSetting("auth0:ClientId", ""),
                Auth0ClientSecret = AppDeployment.Instance.AppSetting("auth0:ClientSecret", "")
            };
            return Ok(model);
        }
    }
}
