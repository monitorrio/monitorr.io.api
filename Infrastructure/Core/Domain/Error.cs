using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Core.Domain
{
    public class Error 
    {
        public ObjectId _id { get; set; }
        public virtual string Guid { get; set; }
        public virtual string ApplicationName { get; set; }
        public virtual string Host { get; set; }
        public virtual string Type { get; set; }
        public virtual string Source { get; set; }
        public virtual string Message { get; set; }
        public virtual string User { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual int StatusCode { get; set; }
        public virtual string WebHostHtmlMessage { get; set; }
        public virtual string Browser { get; set; }
        public virtual string IP { get; set; }
        public virtual string Detail { get; set; }
        public virtual string Url { get; set; }

        public bool IsCustom { get; set; }

        public virtual Dictionary<string, string> ServerVariables { get; set; }
        public virtual Dictionary<string, string> QueryString { get; set; }
        public virtual Dictionary<string, string> Form { get; set; }
        public virtual Dictionary<string, string> Cookies { get; set; }
        public virtual Dictionary<string, string> CustomData { get; set; }

        public virtual string LogId { get; set; }
        public virtual UserMachine UserMachine { get; set; }
        public virtual Severity Severity { get; set; }
        public virtual string Method { get; set; }
    }
}
