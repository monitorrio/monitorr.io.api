using System;
using Core.Domain;

namespace Web.Models
{
    public class RecentErrorModel
    {
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public Severity Severity { get; set; }
        public string Guid { get; set; }

        public RecentErrorModel MapEntity(Error entity)
        {
            Guid = entity.Guid;
            Message = entity.Message;
            Time = entity.Time;
            Severity = entity.Severity;
            return this;
        }
    }
}