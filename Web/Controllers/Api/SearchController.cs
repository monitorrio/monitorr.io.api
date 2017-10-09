using Web.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Core.Domain;
using SharpAuth0;
using Web.Infrastructure.DataTables;
using Web.Infrastructure.Extensions;
using Web.Infrastructure.DataTables.Helpers;
using Web.Infrastructure.Repositories;

namespace Web.Controllers.Api
{
    [Authorize]
    public class SearchController : BaseController
    {
        private IErrorRepository _errorRepository;
        private ILogRepository _logRepository;

        public SearchController(
            IErrorRepository errorRepository,
            ILogRepository logRepository,
            IIdentityGateway identityGateway
            ) : base(identityGateway)
        {
            _logRepository = logRepository;
        }



        [HttpPost]
        public ContentResult LatestErrors(List<DataTableKeyValuePair> data)
        {
            var dtQuery = new DataTableQuery(data);

            var logs = _logRepository.FindAllByUserId(CurrentUserId);
            List<Error> query = _errorRepository.FindLatestByUserId(CurrentUserId);
            List<ErrorModel> items = query.Select(x => new ErrorModel().MapEntity(x)).ToList().ResolveLogNames(logs);

            var results = new CustomResultSet<ErrorModel>
            {
                aaData = items,
                RecordCount = items.Count,
                CurrentPage = dtQuery.PageSize,
                PageSize = dtQuery.PageSize,
                sEcho = dtQuery.sEcho
            };

            return DataTableContentResult<ErrorModel>.ContentResult(results);
        }

        [HttpPost]
        public ContentResult Errors(List<DataTableKeyValuePair> data)
        {
            var dtQuery = new DataTableQuery(data);

            IList<Error> query = string.IsNullOrEmpty(dtQuery.SearchTerm)
                ? _errorRepository.Search(dtQuery.iDisplayStart, dtQuery.iDisplayLength, data)
                : _errorRepository.Search(dtQuery.SearchTerm, data);

            IList<ErrorModel> items = string.IsNullOrEmpty(dtQuery.SearchTerm)
                ? query.Select(x => new ErrorModel().MapEntity(x)).ToList()
                : query.Skip(dtQuery.iDisplayStart)
                    .Take(dtQuery.iDisplayLength)
                    .Select(x => new ErrorModel().MapEntity(x))
                    .ToList();

            var results = new CustomResultSet<ErrorModel>
            {
                aaData = items,
                RecordCount = (int) (string.IsNullOrEmpty(dtQuery.SearchTerm) ? _errorRepository.CountByLogId(data.ToLogId()) :query.Count),
                CurrentPage = dtQuery.PageSize,
                PageSize = dtQuery.PageSize,
                sEcho = dtQuery.sEcho
            };

            return DataTableContentResult<ErrorModel>.ContentResult(results);
        }
    }
}