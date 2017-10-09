using System;
using System.Collections.Generic;
using Core;
using Core.Domain;
using Web.Infrastructure.Extensions;

namespace Web.Models
{
    public class ErrorModel : BaseModel, IModel<ErrorModel, Error>
    {
        public virtual string Guid { get; set; }
        public virtual string LogId { get; set; }
        public virtual string ApplicationName { get; set; }
        public virtual string Host { get; set; }
        public virtual string Type { get; set; }
        public virtual string Source { get; set; }
        public virtual string Message { get; set; }
        public virtual string User { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual string DateCreated { get; set; }
        public virtual string RelativeTime { get; set; }
        public virtual int StatusCode { get; set; }
        public virtual string WebHostHtmlMessage { get; set; }
        public virtual string Browser { get; set; }
        public virtual string IP { get; set; }
        public virtual string Detail { get; set; }
        public bool IsCustom { get; set; }     
        public string Url { get; set; }
        public string Method { get; set; }

        public virtual Dictionary<string, string> ServerVariables { get; set; }
        public virtual Dictionary<string, string> QueryString { get; set; }
        public virtual Dictionary<string, string> Form { get; set; }
        public virtual Dictionary<string, string> Cookies { get; set; }
        public virtual IDictionary<string, string> CustomData { get; set; }
        public Severity Severity { get; set; }

        public virtual LogModel Log { get; set; }

        public ErrorModel MapEntity(Error entity)
        {
            Guid = entity.Guid;
            ApplicationName = entity.ApplicationName;
            Host = entity.Host;
            Type = entity.Type;
            Source = entity.Source;
            Message = entity.Message;
            User = entity.User;
            Time = entity.Time;
            DateCreated = entity.Time.ToFriendly();
            RelativeTime = entity.Time.ToRelativeString(AppTime.Now());
            StatusCode = entity.StatusCode;
            Browser = entity.Browser;
            Detail = entity.Detail;
            ServerVariables = entity.ServerVariables ?? new Dictionary<string, string>();
            QueryString = entity.QueryString ?? new Dictionary<string, string>();
            Form = entity.Form ?? new Dictionary<string, string>();
            Cookies = entity.Cookies ?? new Dictionary<string, string>();
            CustomData = entity.CustomData ?? new Dictionary<string, string>();
            Severity = entity.Severity;
            IsCustom = entity.IsCustom;
            Url = entity.Url;
            Method = entity.Method;
            Log = new LogModel
            {
                LogId = entity.LogId
            };
            return this;
        }
    }
}
