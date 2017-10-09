using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using Web.Infrastructure.DataTables;
using Web.Infrastructure.Extensions;

namespace Web.Infrastructure.Repositories
{
    public class UserRepository : MongoRepository, IUserRepository
    {
        readonly IMongoCollection<User> _collection;
        readonly IMongoCollection<Log> _logCollection;
        internal string CollectionName { get; set; }

        public UserRepository()
        {
            CollectionName = new User().GetType().Name;
            _collection = Database.GetCollection<User>(CollectionName);
            _logCollection = Database.GetCollection<Log>("Log");
        }
        public Task AddAsync(User user)
        {
            return _collection.InsertOneAsync(user);
        }

        public IList<User> SearchForLog(int skip, int take, List<DataTableKeyValuePair> data)
        {
            var logId = data.ToLogId();
            var log = _logCollection.Find(l => l.LogId == logId).Single();
            if (log.UserAccesses == null)
            {
                return new List<User>();
            }

            var logUserIds = log.UserAccesses.Select(u => new ObjectId(u.UserId)).ToList();
            
            var filter = Builders<User>.Filter.In(u=>u._id, logUserIds);
            var sort = Builders<User>.Sort.Descending("LastName");
            return _collection.Find(filter).Sort(sort).Skip(skip).Limit(take).ToList();
        }

        public IList<User> SearchNotAssignedToLog(string query, List<DataTableKeyValuePair> data)
        {
            var logId = data.ToLogId();
            var log = _logCollection.Find(l => l.LogId == logId).Single();
            List<ObjectId> assignedUserIds = new List<ObjectId>();

            if (log.UserAccesses != null)
            {
                assignedUserIds = log.UserAccesses.Select(u => new ObjectId(u.UserId)).ToList();
            }

            assignedUserIds.Add(new ObjectId(log.UserId));
           
            var builder = Builders<User>.Filter;
            var sort = Builders<User>.Sort.Descending("LastName");

            var filter = (builder.Regex("Name", new BsonRegularExpression(query, "i"))
                         | builder.Regex("Email", new BsonRegularExpression(query, "i")))
                         & builder.Nin(u=>u._id, assignedUserIds);

            var byType = _collection.Find(filter).Sort(sort).ToList();
            return byType.ToList();
        }

        public async Task<bool> IsUserActive(string email)
        {
            return await _collection.Find(x => x.Email == email && x.IsActive).AnyAsync();
        }

        public Task<User> GetUserAsync(string email)
        {
            return _collection.Find(u => u.Email == email).SingleOrDefaultAsync();
        }

        public async Task<bool> IsEmailExists(string email)
        {
            return await _collection.Find(x => x.Email == email).AnyAsync();
        }

        public Task<User> GetByAuth0IdAsync(string auth0Id)
        {
            return _collection.Find(u => u.Auth0Id == auth0Id).SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var filter = Builders<User>.Filter.Eq("_id", user._id);
            var update = Builders<User>.Update.Set("Name", user.Name)
                                             .Set("FirstName", user.FirstName)
                                             .Set("LastName", user.LastName)
                                             .Set("DateModified", user.DateModified)
                                             .Set("PictureUrl", user.PictureUrl)
                                             .Set("Auth0Id", user.Auth0Id)
                                             .Set("IsActive", user.IsActive)
                                             .Set("RoleId", user.RoleId);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task<IList<User>> GetByAuth0Id(IList<string> userIds)
        {
            var filter = Builders<User>.Filter.In(u => u.Auth0Id, userIds);
            return await _collection.Find(filter).ToListAsync();
        }

        public string GetIdByAuth0Id(string auth0Id)
        {
            return _collection.Find(c => c.Auth0Id == auth0Id).Project(u=>u._id).SingleOrDefault().ToString();
        }

        public Task<User> GetById(string id)
        {
            return _collection.Find(u => u._id == ObjectId.Parse(id)).SingleOrDefaultAsync();
        }
    }

    public interface IUserRepository
    {
        Task AddAsync(User user);
        IList<User> SearchForLog(int skip, int take, List<DataTableKeyValuePair> data);
        IList<User> SearchNotAssignedToLog(string query, List<DataTableKeyValuePair> data);
        Task<bool> IsUserActive(string email);
        Task<User> GetUserAsync(string email);
        Task<bool> IsEmailExists(string email);
        Task<User> GetByAuth0IdAsync(string auth0Id);
        Task<IList<User>> GetByAuth0Id(IList<string> userIds);
        Task UpdateAsync(User user);
        string GetIdByAuth0Id(string auth0Id);
        Task<User> GetById(string id);
    }
}