namespace Core.Domain
{
    public class Notification:MongoEntity
    {
        public string LogId { get; set; }
        public string UserId { get; set; }
        public bool IsDailyDigestEmail { get; set; }
        public bool IsNewErrorEmail { get; set; }
    }
}
