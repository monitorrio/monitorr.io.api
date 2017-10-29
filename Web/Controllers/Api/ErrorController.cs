using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Web.Models;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharpAuth0;
using Web.Infrastructure.Extensions;
using Web.Infrastructure.Repositories;
using Web.Infrastructure.Services.Email;
using Web.Infrastructure.Static;

namespace Web.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/errors")]
    [Authorize]
    public class ErrorController : BaseApiController
    {
        private readonly ILogRepository _logRepository;
        private readonly IErrorRepository _errorRepository;
        private readonly IEmailSender _emailSender;

        public ErrorController(
            ILogRepository logRepository,
            IErrorRepository errorRepository,
             IIdentityGateway identityGateway,
             IEmailSender emailSender
            ) : base(identityGateway)
        {
            _logRepository = logRepository;
            _errorRepository = errorRepository;
            _emailSender = emailSender;
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var x = await _logRepository.FindByLogIdAsync(id);

            if (x == null)
            {
                return NotFound();
            }

            var model = new LogModel().MapEntity(x).ResolveErrorCount(_errorRepository);
            return Ok(model);
        }

        [HttpGet, Route("{id}/info")]
        public async Task<IHttpActionResult> GetErrorInfo(string id)
        {
            var x = await _errorRepository.GetByGuidAsync(id);

            if (x == null)
            {
                return NotFound();
            }

            var model = new ErrorModel().MapEntity(x);
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return Json(model, serializerSettings);
        }

        [HttpGet, Route("new")]
        public async Task<HttpResponseMessage> GetNew(DateTime? lastChecked = null,
            int limit = 5)
        {
            if (lastChecked == null)
            {
                lastChecked = DateTime.MinValue;
            }

            var totalCount = await _errorRepository.CountNewByUserIdAsync(CurrentUserId, lastChecked.Value);

            var newErrors = await _errorRepository.FindNewByUserIdAsync(CurrentUserId, lastChecked.Value, limit);

            var newErrorsModel = newErrors.Select(e => new NewErrorModel().MapEntity(e)).ToList();
            var model = new NewErrorListModel
            {
                Total = totalCount,
                Errors = newErrorsModel
            };

            return model.ToResult(HttpNotificationStatus.Success.ToString())
                .ToHttpResponseMessageJson();
        }

        [HttpPost, Route("newErrorEmail")]
        [AllowAnonymous]
        public IHttpActionResult SendNewErrorEmail(SendEmailModel model)
        {
            _emailSender.SendAsync(model);
            return Ok();
        }
    }
}
