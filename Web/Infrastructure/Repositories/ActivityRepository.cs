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
    public class ActivityRepository : MongoRepository, IActivityRepository
    {
        readonly IMongoCollection<Log> _collection;
        internal string CollectionName { get; set; }

        public ActivityRepository()
        {
            CollectionName = new Log().GetType().Name;
            _collection = WebMdActivityDatabase.GetCollection<Log>(CollectionName);
        }
    }

    public interface IActivityRepository
    {
    }
}