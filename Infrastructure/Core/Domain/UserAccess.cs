using System;

namespace Core.Domain
{
    public class UserAccess:MongoEntity
    {
        public string UserId { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
    }
}