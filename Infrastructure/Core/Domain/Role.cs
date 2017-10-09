using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Domain
{
    public class Role : MongoEntity
    {
        [BsonRepresentation(BsonType.String)]
        public RoleName RoleName { get; set; }
        public string ApplicationName { get; set; }
    }

    public enum RoleName
    {
        LogAccess = 0,
        Free = 1,
        Pro = 2,
        Enterprise = 3,
        Admin = 100
    }
}
