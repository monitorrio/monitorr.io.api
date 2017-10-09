namespace Web.Models
{
    public class ValidationError
    {
        public string Message { get; set; }
        public string ElementId { get; set; }
        public string CollectionIdentifier { get; set; }
        public string CollectionName { get; set; }
    }
}