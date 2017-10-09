using Core.Domain;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Infrastructure.Repositories
{
    public class NotificationRepository : MongoRepository, INotificationRepository
    {
        readonly IMongoCollection<Notification> _collection;
        internal string CollectionName { get; set; }

        public NotificationRepository()
        {
            CollectionName = new Notification().GetType().Name;
            _collection = Database.GetCollection<Notification>(CollectionName);
        }

        public async Task CreateOrUpdate(Notification notification)
        {
            var updateDefinition = Builders<Notification>.Update.Set("UserId", notification.UserId)
                .Set("LogId", notification.LogId)
                .Set("IsNewErrorEmail", notification.IsNewErrorEmail)
                .Set("IsDailyDigestEmail", notification.IsDailyDigestEmail);

            await _collection.UpdateOneAsync(n => n.LogId == notification.LogId && n.UserId == notification.UserId, updateDefinition, new UpdateOptions { IsUpsert = true });
        }

        public Task<List<string>> FindAllDigestAsync(string userId)
        {
            return _collection.Find(n => n.UserId == userId && n.IsDailyDigestEmail)
                .Project(n => n.LogId)
                .ToListAsync();
        }

        public Task<bool> IsUserSubscribedToDigestAsync(string userId)
        {
            var isExists = _collection
                .Count(l => l.IsDailyDigestEmail && l.UserId == userId) > 0;

            return Task.FromResult(isExists);
        }

        public Task<bool> IsUserSubscribedToDigestAsync(string userId, string logId)
        {
            var isExists = _collection
               .Count(l => l.IsDailyDigestEmail && l.UserId == userId && l.LogId == logId) > 0;

            return Task.FromResult(isExists);
        }

        public Task<bool> IsUserSubscribedToNewErrorAsync(string userId, string logId)
        {
            var isExists = _collection
               .Count(l => l.IsNewErrorEmail && l.UserId == userId && l.LogId == logId) > 0;

            return Task.FromResult(isExists);
        }

        public async Task  Delete(string logId)
        {
            await _collection.DeleteManyAsync(d => d.LogId == logId);
        }
    }

    public interface INotificationRepository
    {
        Task CreateOrUpdate(Notification notification);
        Task<List<string>> FindAllDigestAsync(string userId);
        Task<bool> IsUserSubscribedToDigestAsync(string userId);
        Task<bool> IsUserSubscribedToDigestAsync(string userId, string logId);
        Task<bool> IsUserSubscribedToNewErrorAsync(string userId, string logId);
        Task Delete(string logId);
    }
}