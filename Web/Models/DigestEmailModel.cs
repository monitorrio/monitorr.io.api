namespace Web.Models
{
    public class DigestEmailModel
    {
        public string Date { get; set; }
        public string LogId { get; set; }
        public string LogName { get; set; }
        public long Total { get; set; }
        public long Critical { get; set; }
        public long Unique { get; set; }
    }
}