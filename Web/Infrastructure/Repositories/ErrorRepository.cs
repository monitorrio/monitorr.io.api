using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using Web.Infrastructure.DataTables;
using Web.Infrastructure.Extensions;
using DataTables.AspNet.Core;
using MongoDB.Bson.Serialization;

namespace Web.Infrastructure.Repositories
{
    public class ErrorRepository : MongoRepository, IErrorRepository
    {
        readonly IMongoCollection<Error> _collection;
        readonly IMongoCollection<Log> _logCollection;

        public ErrorRepository()
        {
            _collection = Database.GetCollection<Error>(new Error().GetType().Name);
            _logCollection = Database.GetCollection<Log>(new Log().GetType().Name);
        }

        public Error GetByGuid(string guid)
        {
            return _collection.Find(x => x.Guid == guid).First();
        }

        public List<Error> FindLatestByUserId(string userId)
        {
            var ownerLogs = _logCollection.Find(x => x.UserId == userId).ToList().Select(x => x.LogId).ToList();

            var query = new List<Error>();

            foreach (var logId in ownerLogs)
            {
                var filter = Builders<Error>.Filter.Eq("LogId", logId);
                var sort = Builders<Error>.Sort.Ascending("Time");
                query.AddRange(_collection.Find(filter).Sort(sort).Limit(5).ToList());
            }

            return query.OrderBy(x => x.Time).Take(10).ToList();
        }

        public async Task<List<Error>> FindLatestByUserIdAsync(string userId)
        {
            var ownerLogs = _logCollection.FindSync(x => x.UserId == userId).ToList().Select(x => x.LogId).ToList();

            var query = new List<Error>();

            foreach (var logId in ownerLogs)
            {
                var filter = Builders<Error>.Filter.Eq("LogId", logId);
                var sort = Builders<Error>.Sort.Ascending("Time");
                query.AddRange(await _collection.Find(filter).Sort(sort).Limit(5).ToListAsync());
            }

            return query.OrderBy(x => x.Time).Take(10).ToList();
        }

        public async Task<List<Error>> FindNewByUserIdAsync(string userId, DateTime lastChecked, int limit = 5)
        {
            var ownerLogs = _logCollection.FindSync(x => x.UserId == userId).ToList().Select(x => x.LogId).ToList();

            var filter = Builders<Error>.Filter.In("LogId", ownerLogs)
                         & Builders<Error>.Filter.Gt("Time", lastChecked);

            var sort = Builders<Error>.Sort.Descending("Time");
            return await _collection.Find(filter).Sort(sort).Limit(limit).ToListAsync();
        }

        public async Task ClearByLogId(Guid logId)
        {
            var filter = Builders<Error>.Filter.Eq("LogId", logId.ToString());
            await _collection.DeleteManyAsync(filter);
        }

        public async Task<long> CountNewByUserIdAsync(string userId, DateTime lastChecked)
        {
            var ownerLogs = _logCollection.FindSync(x => x.UserId == userId).ToList().Select(x => x.LogId).ToList();

            var filter = Builders<Error>.Filter.In("LogId", ownerLogs)
                         & Builders<Error>.Filter.Gt("Time", lastChecked);

            return await _collection.CountAsync(filter);
        }

        public Task<List<Error>> FindByLogId(string logId)
        {
            return _collection.Find(x => x.LogId == logId).ToListAsync();
        }

        public List<Error> FindByLogId(string logId, DateTime startDate, DateTime endDate, int? take)
        {
            var builder = Builders<Error>.Filter;
            var sort = Builders<Error>.Sort.Descending("Time");

            var filter = builder.Eq("LogId", logId) & builder.Gt("Time", startDate) & builder.Lt("Time", endDate);

            return take != null ? _collection.Find(filter).Sort(sort).Limit(take).ToList() : _collection.Find(filter).Sort(sort).ToList();
        }

        public int CountByLogId(string logId)
        {
            FilterDefinition<Error> filter = Builders<Error>.Filter.Eq("LogId", logId);
            return Convert.ToInt32(_collection.Count(filter));
        }

        public Task<List<Error>> GetForPeriodAsync(string logId, DateTime? from, DateTime? to)
        {
            FilterDefinition<Error> filter = Builders<Error>.Filter.Eq("LogId", logId);

            if (from != null)
            {
                filter = filter & Builders<Error>.Filter.Gte("Time", from);
            }

            if (to != null)
            {
                filter = filter & Builders<Error>.Filter.Lte("Time", to);
            }

            return _collection.Find(filter).ToListAsync();
        }

        public IList<Error> Search(int skip, int take, List<DataTableKeyValuePair> data)
        {
            string logId = data.Where(n => n.name == "LogId").Select(x => x.value).FirstOrDefault();
            var sort = Builders<Error>.Sort.Descending("Time");

            IFindFluent<Error, Error> query = _collection.Find(x => x.LogId == logId);

            return query.Sort(sort).Skip(skip).Limit(take).ToList();
        }

        public IQueryable<Error> Query()
        {
            return _collection.AsQueryable();
        }

        public Task<List<string>> GetAllUniqueHosts(string logId)
        {
            var filter = Builders<Error>.Filter.Where(e => e.LogId == logId);
            var list = _collection.Distinct<string>("Host", filter).ToList();

            list.Sort();
            return Task.FromResult(list);

        }

        public Task<List<int>> GetAllUniqueStatusCodes(string logId)
        {
            var logIdFilter = Builders<Error>.Filter.Where(e => e.LogId == logId);
            var list = _collection.Distinct<int>("StatusCode", logIdFilter).ToList();

            list.Sort();
            return Task.FromResult(list);
        }

        public Task<List<string>> GetAllUniqueErrorTypes(string logId)
        {
            var filter = Builders<Error>.Filter.Where(e => e.LogId == logId && e.Type != null);
            var list = _collection.Distinct<string>("Type", filter).ToList();

            list.Sort();
            return Task.FromResult(list);
        }

        public Task<List<string>> GetAllBrowsers(string logId)
        {
            var logIdFilter = Builders<Error>.Filter.Where(e => e.LogId == logId);
            var list = _collection.Distinct<string>("Browser", logIdFilter).ToList();

            list.Sort();
            return Task.FromResult(list);
        }

        public IList<Error> Search(string query, List<DataTableKeyValuePair> data)
        {
            var builder = Builders<Error>.Filter;
            FilterDefinition<Error> filter;
            var sort = Builders<Error>.Sort.Descending("Time");

            //by Type
            filter = builder.Eq("LogId", data.ToLogId()) & builder.Regex("Type", new BsonRegularExpression(query));
            var byType = _collection.Find(filter).Sort(sort).ToList();
            if (byType.Any())
            {
                return byType.ToList();
            }

            //by Source
            filter = builder.Eq("LogId", data.ToLogId()) & builder.Regex("Source", new BsonRegularExpression(query));
            var bySource = _collection.Find(filter).Sort(sort).ToList();
            if (bySource.Any())
            {
                return bySource.ToList();
            }

            //by Message
            filter = builder.Eq("LogId", data.ToLogId()) & builder.Regex("Message", new BsonRegularExpression(query));
            var byMessage = _collection.Find(filter).Sort(sort).ToList();
            if (byMessage.Any())
            {
                return byMessage.ToList();
            }

            //by User
            filter = builder.Eq("LogId", data.ToLogId()) & builder.Regex("User", new BsonRegularExpression(query));
            var byUser = _collection.Find(filter).Sort(sort).ToList();
            if (byUser.Any())
            {
                return byUser.ToList();
            }

            //by Ip
            filter = builder.Eq("LogId", data.ToLogId()) & builder.Regex("ServerVariables.REMOTE_ADDR", new BsonRegularExpression(query));
            var byIp = _collection.Find(filter).Sort(sort).ToList();
            if (byIp.Any())
            {
                return byIp.ToList();
            }

            return new List<Error>();
        }

        public Task<long> CountCriticalByLogIdAsync(string logId)
        {
            var oneHourAgo = DateTime.UtcNow.AddHours(-1);
            return _collection.CountAsync(e => e.LogId == logId &&
                                               e.Severity == Severity.Critical &&
                                               e.Time >= oneHourAgo
            );
        }

        public IList<Error> FindByPredicate(Expression<Func<Error, bool>> predicate)
        {
            return _collection.Find(predicate).ToList();
        }

        public Task<long> CountForPeriodAsync(string logId, DateTime from, DateTime to)
        {
            FilterDefinition<Error> filter = Builders<Error>.Filter.Eq("LogId", logId)
               & Builders<Error>.Filter.Gte("Time", from)
               & Builders<Error>.Filter.Lte("Time", to);

            var count = _collection.Count(filter);
            return Task.FromResult(count);
        }
    }

    public interface IErrorRepository
    {
        Error GetByGuid(string guid);
        List<Error> FindLatestByUserId(string userId);
        Task<List<Error>> FindLatestByUserIdAsync(string userId);
        Task<List<Error>> FindByLogId(string logId);
        List<Error> FindByLogId(string logId, DateTime startDate, DateTime endDate, int? take);
        int CountByLogId(string logId);

        Task<List<Error>> GetForPeriodAsync(string logId, DateTime? from, DateTime? to);
        Task<long> CountForPeriodAsync(string logId, DateTime from, DateTime to);

        IList<Error> FindByPredicate(Expression<Func<Error, bool>> predicate);

        IList<Error> Search(int skip, int take, List<DataTableKeyValuePair> data);
        IList<Error> Search(string query, List<DataTableKeyValuePair> data);
        Task<long> CountCriticalByLogIdAsync(string logId);
        Task<long> CountNewByUserIdAsync(string userId, DateTime lastChecked);
        Task<List<Error>> FindNewByUserIdAsync(string userId, DateTime lastChecked, int limit);
        Task ClearByLogId(Guid logId);
        IQueryable<Error> Query();
        Task<List<string>> GetAllUniqueHosts(string logId);
        Task<List<int>> GetAllUniqueStatusCodes(string logId);
        Task<List<string>> GetAllUniqueErrorTypes(string logId);
        Task<List<string>> GetAllBrowsers(string logId);
    }
}
