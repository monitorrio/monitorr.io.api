using Core;
using Core.Domain;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization;
using Web.Models;

namespace Web.Infrastructure.Repositories
{
    public class LogRepository : MongoRepository, ILogRepository
    {
        readonly IMongoCollection<Log> _collection;
        internal string CollectionName { get; set; }

        public LogRepository()
        {
            CollectionName = new Log().GetType().Name;
            _collection = Database.GetCollection<Log>(CollectionName);
        }

        public List<Log> FindAllByUserId(string userId)
        {
            return _collection.Find(x => x.UserId == userId).ToList();
        }

        public async Task<List<Log>> FindAllByUserIdAsync(string userId)
        {
            return await _collection.Find(x => x.UserId == userId 
            || x.UserAccesses.Any(a => a.UserId == userId)).ToListAsync();
        }

        public async Task<List<Log>> FindAllAsync()
        {
            return await _collection.Find(x=>x.Name.Length > 1).ToListAsync();
        }

        public async Task<List<Log>> GetAllAsync()
        {
            return await _collection.AsQueryable().ToListAsync();
        }

        public List<Log> FindByLogId(string logId)
        {
            return _collection.Find(x => x.LogId == logId).ToList();
        }

        public async Task<Log> FindByLogIdAsync(string logId)
        {
            var col =  await _collection.FindAsync(x => x.LogId == logId);
            return await col.FirstAsync();
        }

        public async Task AddAsync(Log x)
        {
            await _collection.InsertOneAsync(x);
        }

        public void Add(Log x)
        {
            _collection.InsertOne(x);
        }

        public void Update(Log x)
        {
            var filter = Builders<Log>.Filter.Eq("LogId", x.LogId);
            var update = Builders<Log>.Update.Set("Name", x.Name)
                                             .Set("Color", x.Color)
                                             .Set("WidgetColor", x.WidgetColor)
                                             .Set("DateModified", x.DateModified)
                                             .Set("UserAccesses", x.UserAccesses);

            _collection.UpdateOne(filter, update);
        }

        public void UpdateOwner(Log x)
        {
            var filter = Builders<Log>.Filter.Eq("LogId", x.LogId);
            var update = Builders<Log>.Update.Set("UserId", x.UserId);
            _collection.UpdateOne(filter, update);
        }

        public async Task UpdateAsync(Log x)
        {
            var filter = Builders<Log>.Filter.Eq("LogId", x.LogId);
            var update = Builders<Log>.Update.Set("Name", x.Name)
                                             .Set("Color", x.Color)
                                             .Set("WidgetColor", x.WidgetColor)
                                             .Set("DateModified", x.DateModified)
                                             .Set("UserAccesses", x.UserAccesses);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task<Log> FindByNameAndUserIdAsync(string name,string userId)
        {
            var col = await _collection.FindAsync(x => x.UserId == userId && x.Name == name);
            return await col.FirstAsync();
        }

        public async Task<bool> LogAlreadyExistsAsync(string name, string userId)
        {
            var col = await _collection.FindAsync(x => x.UserId == userId
                                                       && x.Name == name);
            return await col.AnyAsync();
        }

        public async Task DeleteAsync(string logId)
        {
            var filter = Builders<Log>.Filter.Eq("LogId", logId);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task ReassignLogsOwnerAsync(string currentOwnerId, string newOwnerId)
        {
            var items = await _collection.Find(x => x.UserId == currentOwnerId).ToListAsync();

            foreach (var item in items)
            {
                var filter = Builders<Log>.Filter.Eq("LogId", item.LogId);
                var update = Builders<Log>.Update.Set("Owner.UserId", newOwnerId).Set("DateModified", AppTime.Now());

               await _collection.UpdateOneAsync(filter,update);
            }
        }

        public async Task UpdateAccess(LogAccessModel model)
        {
            var log = await FindByLogIdAsync(model.LogId);
            if (log.UserAccesses == null)
            {
                log.UserAccesses = new List<UserAccess>();
            }

            var currentAccess = log.UserAccesses.SingleOrDefault(a => a.UserId == model.UserId);

            if (currentAccess == null)
            {
                log.UserAccesses.Add(new UserAccess
                {
                    CanRead = model.CanRead,
                    CanWrite = model.CanWrite,
                    UserId = model.UserId
                });
            }
            else
            {
                currentAccess.CanRead = model.CanRead;
                currentAccess.CanWrite = model.CanWrite;
            }

            await UpdateAsync(log);
        }       

        private static void RegisterClassMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(new Log().GetType()))
            {
                BsonClassMap.RegisterClassMap<Log>(y => y.AutoMap());
            }
        }

        public async Task<bool> IsOwnerAsync(string logId, string userId)
        {
            var log = await _collection.Find(l => l.LogId == logId && l.UserId == userId).SingleOrDefaultAsync();
            return log != null;
        }

        public async Task<List<Log>> FindByLogIdAsync(List<string> logIds)
        {
            var filter = Builders<Log>.Filter.In(u => u.LogId, logIds);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<string> GetName(string logId)
        {
            return await _collection.Find(f=>f.LogId == logId).Project(l=>l.Name).SingleAsync();
        }
    }

    public interface ILogRepository
    {
        Task<string> GetName(string logId);
        List<Log> FindAllByUserId(string userId);
        Task<List<Log>> FindAllByUserIdAsync(string userId);
        Task<List<Log>> FindAllAsync();
        Task<List<Log>> GetAllAsync();
        List<Log> FindByLogId(string logId);
        Task<Log> FindByLogIdAsync(string logId);
        Task AddAsync(Log x);
        void Add(Log x);
        void Update(Log x);
        void UpdateOwner(Log x);
        Task UpdateAsync(Log x);
        Task<Log> FindByNameAndUserIdAsync(string name, string userId);
        Task<bool> LogAlreadyExistsAsync(string name, string userId);
        Task DeleteAsync(string logId);
        Task ReassignLogsOwnerAsync(string currentOwnerId, string newOwnerId);
        Task UpdateAccess(LogAccessModel model);
        Task<bool> IsOwnerAsync(string logId, string userId);
        Task<List<Log>> FindByLogIdAsync(List<string> logIds);
    }
}
