using Core.Domain;

namespace Web.Models
{
    public class LogAccessModel
    {
        public string LogId { get; set; }

        public string UserId { get; set; }

        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
    }
}