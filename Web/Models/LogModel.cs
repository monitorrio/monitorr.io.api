using System;
using System.Collections.Generic;
using Core.Domain;

namespace Web.Models
{
    public class LogModel : BaseModel, IModel<LogModel, Log>
    {
        public virtual string LogId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Color { get; set; }
        public string WidgetColor { get; set; }
        public virtual string ErrorCount { get; set; }
        public long LatestErrorCount { get; set; }
        public virtual long SpaceUsedPercentage { get; set; }
        public virtual List<int> DataPoints { get; set; }
        public virtual IList<ErrorModel> Errors { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsDailyDigestEmail { get; set; }
        public bool IsNewErrorEmail { get; set; }
        public string ShortName { get; set; }
        public int IncreasedSevenDaysPercentage { get; set; }
        public long CriticalCount { get; set; }

        public LogModel MapEntity(Log entity)
        {
            LogId = entity.LogId;
            Name = entity.Name;
            WidgetColor = entity.WidgetColor;
            DateCreated = entity.DateCreated;
            DateModified = entity.DateModified;
            ShortName = entity.ShortName;
            IncreasedSevenDaysPercentage = 10;
            UserId = entity.UserId;
            
            return this;
        }

    }
}
