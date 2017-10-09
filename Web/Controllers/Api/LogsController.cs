using Core;
using System;
using SharpAuth0;
using Web.Models;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Web.Infrastructure.Static;
using System.Collections.Generic;
using System.Web.Http.Cors;
using Core.Domain;
using Web.Infrastructure.DataTables;
using Web.Infrastructure.DataTables.Helpers;
using Web.Infrastructure.Extensions;
using Web.Infrastructure.Repositories;
using Web.Infrastructure.Services;
using DataTables.AspNet.Core;
using DataTables.AspNet.WebApi2;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Web.Infrastructure;

namespace Web.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/logs")]
    [Authorize]
    public class LogsController : BaseApiController
    {
        private readonly ILogRepository _logRepository;
        private readonly IErrorRepository _errorRepository;
        private readonly IJobRunner _jobRunner;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private INotificationRepository _notificationRepository;

        public LogsController(
            IRoleRepository roleRepository,
            ILogRepository logRepository,
            IErrorRepository errorRepository,
            IIdentityGateway identityGateway,
            IJobRunner jobRunner,
            IUserRepository userRepository,
            INotificationRepository notificationRepository
        ) : base(identityGateway)
        {
            _logRepository = logRepository;
            _errorRepository = errorRepository;
            _jobRunner = jobRunner;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _notificationRepository = notificationRepository;
        }


        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var query = await _logRepository.FindAllByUserIdAsync(CurrentUserId);

                this.LogInfo($" logged in user is: {CurrentUserId}");

                if (query == null)
                {
                    return InternalServerError();
                }
                if (!query.Any())
                {
                    return Ok(new List<LogModel>());
                }

                List<LogModel> model = query
                    .Select(x => new LogModel().MapEntity(x))
                    .ToList()
                    .ResolveErrorCount(_errorRepository)
                    .GetNotifications(_notificationRepository, CurrentUserId)
                    .GetSpaceUsed(_errorRepository)
                    .ToToday(_errorRepository);

                return Ok(model);
            }
            catch (Exception ex)
            {
                LogVerbose(ex);
            }
            return InternalServerError();
        }

        [HttpGet, Route("isOwner/{logId}")]
        public async Task<IHttpActionResult> IsOwner(string logId)
        {
            var isOwner = await _logRepository.IsOwnerAsync(logId, CurrentUserId);
            return Ok(isOwner);
        }

        [HttpGet, Route("{logId}/name")]
        public async Task<IHttpActionResult> GetName(string logId)
        {
            var logName = await _logRepository.GetName(logId);
            return Ok(logName);
        }

        [HttpGet, Route("{id}")]
        public async Task<HttpResponseMessage> Get(string id)
        {
            try
            {
                var x = await _logRepository.FindByLogIdAsync(id);

                if (x == null)
                {
                    this.LogInfo($"{CurrentUserId} tried to retrieve a log that was not found id:  {id}");
                    return
                        id.ToResult(HttpNotificationStatus.Warning.ToString(), "Looks like this log has been deleted.")
                            .ToHttpResponseMessageJson();
                }

                var model = new LogModel().MapEntity(x).ResolveErrorCount(_errorRepository);

                this.LogInfo($"{CurrentUserId} updatd log with id {model.LogId}");
                return
                    model.ToResult(HttpNotificationStatus.Success.ToString(), "Log updated.")
                        .ToHttpResponseMessageJson();
            }
            catch (Exception ex)
            {
                this.LogInfo($"{CurrentUserId} tried to retrive log with error:  {ex.InnerException.Message}");
                return
                    id.ToResult(HttpNotificationStatus.Error.ToString(), ex.InnerException.Message)
                        .ToHttpResponseMessageJson();
            }
        }

        [HttpGet, Route("{id}/overview")]
        public async Task<HttpResponseMessage> Overview(string id, DateTime? from = null, DateTime? to = null)
        {
            var errors = await _errorRepository.GetForPeriodAsync(id, from, to);

            if (errors.Count == 0)
            {
                return new LogOverviewModel().ToResult().ToHttpResponseMessageJson();
            }

            var errorsGroupedByBrowser = errors.GroupBy(g => g.Browser)
                .Select(g => g.Key)
                .ToList();

            var errorsGroupedByMessage = errors.GroupBy(e => e.Message);
            var warningPercentage = errors.Count(e => e.Severity == Severity.Warning) * 100 / errors.Count;

            var errorPercentage = 100 - warningPercentage;

            var limit = 5;

            var recentErrors = errors.OrderByDescending(e => e.Time).Take(limit);

            var recentErrorsModel = recentErrors.Select(x => new RecentErrorModel().MapEntity(x)).ToList();

            var frequentErrors = errors
                .GroupBy(e => new { e.Message, e.Severity })
                .OrderByDescending(e => e.Count())
                .Take(limit)
                .Select(e => new FrequentErrorModel
                {
                    Count = e.Count(),
                    Message = e.Key.Message,
                    Severity = e.Key.Severity
                });

            var frequentUrls = errors
                .GroupBy(e => new {e.Url, e.Severity })
                .OrderByDescending(g => g.Count())
                .Take(limit)
                .Select(e => new FrequentUrlModel
                {
                    Count = e.Count(),
                    Message = e.Key.Url,
                    Severity = e.Key.Severity
                });

            var model = new LogOverviewModel
            {
                BrowsersAffected = errorsGroupedByBrowser.Count(),
                TotalErrors = errors.Count,
                CriticalErrors = errors.Count(e => e.Severity == Severity.Critical),
                UsersAffected = errors.GroupBy(d => d.User).Count(),
                UniqueErrors = errorsGroupedByMessage.Count(),
                //TODO consider changing Browser property from string to enum
                IsChromeAffected = errorsGroupedByBrowser.Contains("Chrome"),
                IsFirefoxAffected = errorsGroupedByBrowser.Contains("Firefox"),
                IsIeAffected = errorsGroupedByBrowser.Contains("IE"),
                IsOperaAffected = errorsGroupedByBrowser.Contains("Opera"),
                IsSafariAffected = errorsGroupedByBrowser.Contains("Safari"),
                WarningPercent = warningPercentage,
                ErrorPercentage = errorPercentage,
                RecentErrors = recentErrorsModel,
                FrequentUrls = frequentUrls,
                FrequentErrors = frequentErrors
            };

            return model.ToResult(HttpNotificationStatus.Success.ToString()).ToHttpResponseMessageJson();

        }

        [HttpGet, Route("{id}/overview/aggregate/")]
        public async Task<HttpResponseMessage> AggregatedErrorsCount(string id, double timeZoneOffset, DateTime? from = null,
            DateTime? to = null)
        {
            to = to?.ToUniversalTime() ?? DateTime.UtcNow;

            var errors = await _errorRepository.GetForPeriodAsync(id, from, to);

            from = from?.ToUniversalTime() ?? errors.Min(c => c.Time.ToUniversalTime());

            var model = new List<ErrorCountForPeriodModel>();

            var diff = to - from;

            var format = "";
            var showTimeOnly = diff.Value.TotalDays < 3;
            format = diff.Value.TotalDays > 3 
                ? "d" 
                : "t";

            var groupedByDate = errors.GroupBy(g => g.Time.ToUniversalTime()
            .AddHours(timeZoneOffset).ToString(format)).ToList();

            var myspace = diff.Value.Ticks / 20;

            for (var i = from.Value.Ticks; i < to.Value.Ticks; i = i + myspace)
            {
                var date = new DateTime(i);
                var formatedDate = date.AddHours(timeZoneOffset).ToString(format);
                if (model.Any(m => m.PeriodName == date.ToString(format)))
                {
                    continue;
                }
                var errDate = groupedByDate.SingleOrDefault(s => s.Key == formatedDate);

                model.Add(new ErrorCountForPeriodModel
                {
                    PeriodDate = date,
                    PeriodName = formatedDate,
                    Count = errDate?.Count() ?? 0,
                    ShowTimeOnly = showTimeOnly
                });
            }

            foreach (var error in groupedByDate)
            {
                if (model.Any(m => m.PeriodName == error.Key))
                {
                    continue;
                }

                model.Add(new ErrorCountForPeriodModel
                {
                    PeriodDate = DateTime.Parse(error.Key),
                    PeriodName = error.Key,
                    Count = error.Count(),
                    ShowTimeOnly = showTimeOnly
                });
            }

            model = model.CustomSort(SortingDirection.Ascending, m => m.PeriodDate).ToList();

            return model.ToResult(HttpNotificationStatus.Success.ToString()).ToHttpResponseMessageJson();
        }

        [HttpGet, Route("users/{userId}")]
        public async Task<IHttpActionResult> UserLogs(string userId)
        {
            var query = await _logRepository.FindAllByUserIdAsync(userId);
            List<LogModel> model = query.Select(x => new LogModel().MapEntity(x)).ToList();
            foreach (var log in model)
            {
                log.ErrorCount = _errorRepository.CountByLogId(log.LogId).ToString();
            }
            object logs = new { items = model };
            return Ok(logs);
        }

        //create
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Save(LogModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                {
                    return BadRequest(ResponseMessages.LogNameRequired.ToDesc());
                }

                if (await _logRepository.LogAlreadyExistsAsync(model.Name, CurrentUserId))
                {
                    return BadRequest(ResponseMessages.LogNameDuplicate.ToDesc());
                }

                var user = await _userRepository.GetById(CurrentUserId);
                if (user == null)
                {
                    return BadRequest(ResponseMessages.UserNotFound.ToDesc());
                }
                var x = model.ToNewEntity();

                x.UserId = user._id.ToString();
                await _logRepository.AddAsync(x);

                var notification = CreateNotification(x, model);
                await _notificationRepository.CreateOrUpdate(notification);

                if (model.IsDailyDigestEmail)
                {
                    _jobRunner.CreateDigestEmailJobIfNotExists(notification);
                }

                return Ok(ResponseMessages.LogAdded.ToDesc());

            }
            catch (Exception ex)
            {
                LogVerbose(ex);
            }
            return InternalServerError();
        }

        private Notification CreateNotification(Log x, LogModel model)
        {
            return new Notification
            {
                LogId = x.LogId,
                UserId = CurrentUserId,
                IsDailyDigestEmail = model.IsDailyDigestEmail,
                IsNewErrorEmail = model.IsNewErrorEmail
            };
        }

        //update
        [HttpPut, Route("")]
        public async Task<IHttpActionResult> Update(LogModel model)
        {
            try
            {
                var log = await _logRepository.FindByLogIdAsync(model.LogId);
                if (log == null)
                {
                    return BadRequest(ResponseMessages.LogNotFound.ToDesc());
                }

                if (log.Name != model.Name && await _logRepository.LogAlreadyExistsAsync(model.Name, log.UserId))
                {
                    return BadRequest(ResponseMessages.LogNameDuplicate.ToDesc());
                }

                log.WidgetColor = model.WidgetColor;
                log.Name = model.Name;
                log.DateModified = AppTime.Now();

                await _logRepository.UpdateAsync(log);

                var notification = CreateNotification(log, model);
                await _notificationRepository.CreateOrUpdate(notification);

                var isExist = await _notificationRepository.IsUserSubscribedToDigestAsync(notification.UserId);
                if (isExist)
                {
                    _jobRunner.CreateDigestEmailJobIfNotExists(notification);
                }
                else
                {
                    _jobRunner.DeleteDigestEmailJobIfExists(notification);
                }

                this.LogInfo(IsDbUser
                    ? $"{CurrentUser.FullName} updatd log with id {model.LogId}"
                    : $"{Auth0User.Email} updated log with id {model.LogId}");

                return Ok(ResponseMessages.LogAdded.ToDesc());
            }
            catch (Exception ex)
            {
                this.LogInfo($"{CurrentUserId} tried to updated log with error:  {ex.InnerException.Message}");
            }
            return InternalServerError();
        }

        [HttpDelete, Route("")]
        public async Task<IHttpActionResult> Delete(string logId)
        {
            try
            {
                var log = await _logRepository.FindByLogIdAsync(logId);

                var isOwner = log.UserId == CurrentUserId;
                if (!isOwner)
                {
                    return BadRequest(ResponseMessages.OnlyOwnerCanPerformClearing.ToDesc());
                }
                 
                await _logRepository.DeleteAsync(logId);
                await _notificationRepository.Delete(logId);

                var isSubscribed = await _notificationRepository
                    .IsUserSubscribedToDigestAsync(CurrentUserId);

                if (!isSubscribed)
                {
                    _jobRunner.DeleteDigestEmailJobIfExists(CurrentUserId);
                }

                this.LogInfo($"{CurrentUserId} has deleted log with id {logId}");
                return Ok("Log has been deleted.");
            }
            catch (Exception ex)
            {
                this.LogInfo($"{CurrentUserId} tried to delete log with error: {ex.InnerException.Message}");
                LogVerbose(ex);
            }
            return InternalServerError();

        }

        [AllowAnonymous]
        [HttpGet, Route("search")]
        public async Task<DataTablesJsonResult> Search(IDataTablesRequest request, string logId, 
            Severity? severity, string browser, bool withUserOnly, DateTime? start, DateTime? end,
            string host, int? statusCode, string type)
        {
            var data = _errorRepository.Query().Where(e => e.LogId == logId);
            IQueryable<Error> dataFiltered = data;
            var pred = PredicateBuilder.True<Error>();
            if (severity.HasValue)
            {
                pred = pred.And(e => e.Severity == severity.Value);
            }
            if (!string.IsNullOrEmpty(browser))
            {
                pred = pred.And(e => e.Browser.ToLower() == browser.ToLower());
            }
            if (withUserOnly)
            {
                pred = pred.And(e => e.User != null && e.User != "");
            }
            if (start.HasValue)
            {
                pred = pred.And(e => e.Time >= start);
            }
            if (end.HasValue)
            {
                pred = pred.And(e => e.Time <= end);
            }
            if (statusCode.HasValue)
            {
                pred = pred.And(e => e.StatusCode == statusCode.Value);
            }
            if (!string.IsNullOrWhiteSpace(host))
            {
                pred = pred.And(e => e.Host == host);
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                pred = pred.And(e => e.Type == type);
            }
            if (!string.IsNullOrWhiteSpace(request.Search.Value))
            {
                var searchTerm = request.Search.Value.ToLower();
                var firstSearchPredicate = PredicateBuilder.Create<Error>(d =>
                    d.Host.ToLower().Contains(searchTerm) |
                    d.ServerVariables["Url"].ToLower().Contains(searchTerm) |
                    d.CustomData.ContainsKey(request.Search.Value) |
                    d.Message.ToLower().Contains(searchTerm));


                int intValue;
                if (int.TryParse(request.Search.Value, out intValue))
                {
                    firstSearchPredicate.Or(d => d.StatusCode == intValue);
                }

                pred = pred.And(firstSearchPredicate);
            }

            dataFiltered = dataFiltered.Where(pred);

            var orderColums = request.Columns.Where(x => x.Sort != null);

            var dataPage = dataFiltered.OrderBy(orderColums).Skip(request.Start).Take(request.Length);

            var dto = dataPage.ToList().Select(x => new ErrorModel().MapEntity(x)).ToList();

            var allHosts = await _errorRepository.GetAllUniqueHosts(logId);
            var allStatusCodes = await _errorRepository.GetAllUniqueStatusCodes(logId);
            var allErrorTypes = await _errorRepository.GetAllUniqueErrorTypes(logId);
            var allBrowsers = await _errorRepository.GetAllBrowsers(logId);

            var filters = new FiltersModel
            {
                Hosts = allHosts,
                StatusCodes = allStatusCodes,
                Types = allErrorTypes,
                Browsers = allBrowsers
            };

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var filtersJson = JsonConvert.SerializeObject(filters, serializerSettings);
            var additionalpara = new Dictionary<string, object> {{"filters", filtersJson}};
            var response = DataTablesResponseEx.Create(request, data.Count(), 
                dataFiltered.Count(), dto, additionalpara);

            return new DataTablesJsonResult(response, Request);
        }

        [HttpPost, Route("AddUserAccess")]
        public async Task<IHttpActionResult> AddUserAccess(LogAccessModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserId))
                {
                    return BadRequest(ResponseMessages.LogManageUserUserIdRequired.ToDesc());
                }

                if (string.IsNullOrEmpty(model.LogId))
                {
                    return BadRequest(ResponseMessages.LogManageUserLogIdRequired.ToDesc());
                }

                var isOwner = await _logRepository.IsOwnerAsync(model.LogId, CurrentUserId);
                if (!isOwner)
                {
                    return BadRequest(ResponseMessages.OnlyOwnerCanPerformPermissionSettings.ToDesc());
                }

                await _logRepository.UpdateAccess(model);

                return Ok(ResponseMessages.LogManageUserAccessChanged.ToDesc());

            }
            catch (Exception ex)
            {
                LogVerbose(ex);
            }
            return InternalServerError();
        }

        [HttpPost, Route("clear")]
        public async Task<IHttpActionResult> Clear(ClearLogModel model)
        {
            try
            {
                var isOwner = await _logRepository.IsOwnerAsync(model.LogId.ToString(), CurrentUserId);
                if (!isOwner)
                {
                    return BadRequest(ResponseMessages.OnlyOwnerCanPerformClearing.ToDesc());
                }
                await _errorRepository.ClearByLogId(model.LogId);
                return Ok(ResponseMessages.LogWasCleared.ToDesc());
            }
            catch (Exception ex)
            {
                LogVerbose(ex);
            }
            return InternalServerError();
        }
    }

    public class FiltersModel
    {
        public List<string> Hosts { get; set; }
        public List<string> Types { get; internal set; }
        public List<int> StatusCodes { get; set; }
        public List<string> Browsers { get; set; }
    }
}