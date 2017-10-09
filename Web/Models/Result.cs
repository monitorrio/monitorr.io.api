using System.Collections.Generic;
using System.Net;

namespace Web.Models
{
    public class Result
    {
        public bool Success { get; set; }
        public Message Message { get; set; }
        public string RedirectUrl { get; set; }
        public string RunAfter { get; set; }
        public bool RedirectAfterValidate { get; set; }
        public object Model { get; set; }
        public object UiSupportModels { get; set; }
        public int ModelId { get; set; }
        public Dictionary<string, string> FormModel { get; set; }
        public bool ModelIsForForm { get; set; }
        public string ViewHtml { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
