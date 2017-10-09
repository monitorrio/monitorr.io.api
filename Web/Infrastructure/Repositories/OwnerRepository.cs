using Core.Domain;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Web.Infrastructure.Repositories
{
    public class OwnerRepository : MongoRepository, IOwnerRepository
    {
        readonly IMongoCollection<Color> _collection;

        public OwnerRepository()
        {
            RegisterClassMaps();
            _collection = Database.GetCollection<Color>(new Color().GetType().Name);
        }

        private static void RegisterClassMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(new Color().GetType()))
            {
                BsonClassMap.RegisterClassMap<Color>(y => y.AutoMap());
            }
        }

        public void DeleteAll()
        {
            Database.DropCollection(new Color().GetType().Name);
        }
    }

    public interface IOwnerRepository
    {
        void DeleteAll();
    }
}
