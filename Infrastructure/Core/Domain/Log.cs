using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Log : MongoEntity
    {
        public string LogId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string WidgetColor { get; set; }
        public  DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public string UserId { get; set; }
        public string ShortName { get; set; }

        public List<UserAccess> UserAccesses { get; set; }
    }
}
