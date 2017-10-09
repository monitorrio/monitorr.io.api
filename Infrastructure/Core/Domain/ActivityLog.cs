using System;

namespace Core.Domain
{
    public class ActivityLog : MongoEntity
    {
        public virtual DateTime DateCreated { get; set; }
        public virtual string EntityName { get; set; }
        public virtual string Message { get; set; }
        public virtual string MachineName { get; set; }
        public virtual string Duration { get; set; }

        public virtual string Area { get; set; }
        public virtual string Controller { get; set; }
        public virtual string Action { get; set; }
        public virtual string QueryString { get; set; }

        public virtual UserMachine UserMachine { get; set; }
    }
}
