using MongoDB.Bson;

namespace Core.Domain
{
    public class Color
    {
        public ObjectId _id { get; set; }
        public string Name { get; set; }
    }
}
