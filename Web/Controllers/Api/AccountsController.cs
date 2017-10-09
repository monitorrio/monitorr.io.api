using System;
using System.Configuration;
using System.Threading.Tasks;
using Web.Models;
using System.Web.Http;
using System.Web.Http.Cors;
using Core;
using Core.Domain;
using SharpAuth0;
using Web.Infrastructure.EmailTemplates;
using Web.Infrastructure.Extensions;
using Web.Infrastructure.Repositories;
using Web.Infrastructure.Services;
using Web.Infrastructure.Services.Email;
using Web.Infrastructure.Static;

namespace Web.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/accounts")]
    [Authorize]
    public class AccountsController : BaseApiController
    {
        private readonly IIdentityGateway _identityGateway;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly ILogRepository _logRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailTemplateRenderer _emailTemplateRenderer;

        public AccountsController(IIdentityGateway identityGateway,
            IUserRepository userRepository,
            IEmailSender emailSender,
            ILogRepository logRepository,
            IRoleRepository roleRepository,
            IEmailTemplateRenderer emailTemplateRenderer
            ) : base(identityGateway)
        {
            _identityGateway = identityGateway;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _logRepository = logRepository;
            _roleRepository = roleRepository;
            _emailTemplateRenderer = emailTemplateRenderer;
        }

        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            Auth0User = _identityGateway.FindIdentiyClaims();
            if (Auth0User == null) return Ok("Not Found");
            var model = new UserModel().MapAuth0User(Auth0User);
            return Ok(model);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Post([FromBody]UserModel model)
        {
            var user = await _userRepository.GetUserAsync(model.Email);
            var freeRole = await _roleRepository.GetRoleAsync(RoleName.Free);

            if (user != null)
            {
                user.UpdateFromModel(model);
                if (string.IsNullOrEmpty(user.RoleId))
                {
                    user.RoleId = freeRole._id.ToString();
                }
                user.IsActive = true;
                await _userRepository.UpdateAsync(user);

                return Ok(user._id.ToString());
            }
            var newUser = model.ToNewLicensedUser(model);
            newUser.RoleId = freeRole._id.ToString();
            await _userRepository.AddAsync(newUser);

            return Ok(newUser._id.ToString());
        }


        [HttpPost, Route("invite")]
        public async Task<IHttpActionResult> Invite(InviteUserModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email))
                {
                    return BadRequest(ResponseMessages.InviteUserEmailRequired.ToDesc());
                }

                if (await _userRepository.IsUserActive(model.Email))
                {
                    return BadRequest(ResponseMessages.EmailDuplicate.ToDesc());
                }

                if (!await _userRepository.IsEmailExists(model.Email))
                {
                    await _userRepository.AddAsync(new User
                    {
                        IsActive = false,
                        Email = model.Email
                    });
                }

                var user = await _userRepository.GetUserAsync(model.Email);
                if (user == null)
                {
                    return BadRequest(ResponseMessages.UserNotFound.ToDesc());
                }

                await _logRepository.UpdateAccess(new LogAccessModel
                {
                    LogId = model.LogId,
                    UserId = user._id.ToString(),
                    CanRead = model.CanRead,
                    CanWrite = model.CanWrite
                });

                var body = _emailTemplateRenderer.Render(EmailTemplateName.InviteUser, new
                {
                    ClientDomain = ConfigurationManager.AppSettings["ClientDomain"],
                });

                var sendEmailModel = new SendEmailModel
                {
                    Body = body,
                    Subject = "You has been invited to monitorr.io",
                    Emails = { model.Email }
                };

                await _emailSender.SendAsync(sendEmailModel);

                return Ok(ResponseMessages.Ok.ToDesc());
            }
            catch (Exception ex)
            {
                LogVerbose(ex);
            }
            return InternalServerError();
        }

        [HttpPost, Route("reinvite")]
        public async Task<IHttpActionResult> InviteAgain(InviteUserModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserId))
                {
                    return BadRequest(ResponseMessages.UserNotFound.ToDesc());
                }

                var user = await _userRepository.GetById(model.UserId);

                if (user == null || user.IsActive)
                {
                    return BadRequest(ResponseMessages.UserCannotBeInvited.ToDesc());
                }

                var body = _emailTemplateRenderer.Render(EmailTemplateName.InviteUser, new
                {
                    ClientDomain = ConfigurationManager.AppSettings["ClientDomain"],
                });

                var sendEmailModel = new SendEmailModel
                {
                    Body = body,
                    Subject = "You has been invited to monitorr.io",
                    Emails = { user.Email }
                };

                await _emailSender.SendAsync(sendEmailModel);

                return Ok(ResponseMessages.Ok.ToDesc());
            }
            catch (Exception ex)
            {
                LogVerbose(ex);
            }
            return InternalServerError();
        }
    }
}
