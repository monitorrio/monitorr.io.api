using System.Configuration;
using MongoDB.Driver;

namespace Web.Infrastructure.Repositories
{
    public class MongoRepository
    {
        public IMongoClient MongoClient => new MongoClient(ConfigurationManager.AppSettings["MongoUri"]);
        public IMongoDatabase Database => MongoClient.GetDatabase(ConfigurationManager.AppSettings["monitorrdb"]);
    }
}
