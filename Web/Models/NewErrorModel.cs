using Core.Domain;

namespace Web.Models
{
    public class NewErrorModel
    {
        public string Guid { get; set; }
        public string Message { get; set; }
        public Severity Severity { get; set; }

        public NewErrorModel MapEntity(Error entity)
        {
            Guid = entity.Guid;
            Message = entity.Message;
            Severity = entity.Severity;
            return this;
        }
    }
}