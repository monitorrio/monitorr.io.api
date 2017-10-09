using Core.Domain;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Web.Infrastructure.Repositories
{
    public class ColorRepository : MongoRepository, IColorRepository
    {
        readonly IMongoCollection<Color> _collection;

        public ColorRepository()
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

        public Color GetByName(string name)
        {
            return _collection.Find(x => x.Name == name).First();
        }

        public void Insert(Color x)
        {
            _collection.InsertOne(x);
        }

        public void Delete(Color x)
        {
            var filter = Builders<Color>.Filter.Eq("Name", x.Name);
            _collection.DeleteOne(filter);
        }

        public void DeleteAll()
        {
            Database.DropCollection(new Color().GetType().Name);
        }
    }

    public interface IColorRepository
    {
        Color GetByName(string name);
        void Insert(Color x);
        void Delete(Color x);
        void DeleteAll();
    }
}
