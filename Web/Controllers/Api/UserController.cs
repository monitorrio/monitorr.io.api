using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Core.Domain;
using SharpAuth0;
using Web.Infrastructure.DataTables.Helpers;
using Web.Infrastructure.Extensions;
using Web.Infrastructure.Repositories;
using Web.Infrastructure.Services.Email;
using Web.Infrastructure.Static;
using Web.Models;

namespace Web.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Users")]
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogRepository _logRepository;
        private readonly IEmailSender _emailSender;

        public UsersController(
            IIdentityGateway identityGateway,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ILogRepository logRepository
            ) : base( identityGateway)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _logRepository = logRepository;
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            return Ok();
        }

        [HttpPost, Route("search")]
        public async Task<IHttpActionResult> SearchForLog(object postQuery)
        {
            var data = postQuery.ToKeyValuePair();
            try
            {
                DataTableQuery dtQuery = new DataTableQuery(data);
                IList<User> query = string.IsNullOrEmpty(dtQuery.SearchTerm)
                    ? _userRepository.SearchForLog(dtQuery.iDisplayStart, dtQuery.iDisplayLength, data)
                    : _userRepository.SearchNotAssignedToLog(dtQuery.SearchTerm, data);

                IList<UserLogAccessModel> items = string.IsNullOrEmpty(dtQuery.SearchTerm)
                    ? query.Select(x => new UserLogAccessModel().MapEntity(x)).ToList()
                    : query.Skip(dtQuery.iDisplayStart)
                        .Take(dtQuery.iDisplayLength)
                        .Select(x => new UserLogAccessModel().MapEntity(x)).ToList()
                        .ToList();

                var logId = data.ToLogId();
                var log = await _logRepository.FindByLogIdAsync(logId);
                if (log.UserAccesses != null && log.UserAccesses.Any())
                {
                    FillLogAccess(items, log, logId);
                }

                var dto = new DataTableDto<UserLogAccessModel>
                {
                    aaData = items,
                    RecordCount =query.Count,
                    CurrentPage = dtQuery.PageSize,
                    PageSize = dtQuery.PageSize,
                    sEcho = dtQuery.sEcho
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                LogVerbose(ex);
                return BadRequest();
            }
        }

        private void FillLogAccess(IList<UserLogAccessModel> items, Log log, string logId)
        {
            foreach (var userLogAccessModel in items)
            {
                var access = log.UserAccesses.
                    SingleOrDefault(a => a.UserId == userLogAccessModel.Id);

                if (access != null)
                {
                    userLogAccessModel.LogAccess = new LogAccessModel
                    {
                        CanRead = access.CanRead,
                        CanWrite = access.CanWrite,
                        UserId = access.UserId,
                        LogId = logId
                    };
                }
            }
        }

        //create
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Save(UserModel model)
        {
            try
            {
                var user = model.ToNewLicensedUser(model);
                var freeRole = await _roleRepository.GetRoleAsync(RoleName.Free);
                user.RoleId = freeRole._id.ToString();
                await _userRepository.AddAsync(user);
                return Ok(ResponseMessages.UserAdded.ToDesc());

            }
            catch (Exception ex)
            {
                LogVerbose(ex);
            }
            return InternalServerError();
        }

    }
}