namespace Web.Models
{
    public class Message
    {
        public bool IsSystem { get; set; }
        public bool IsNotify { get; set; }
        public bool DisplayMessage { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        Success,
        Info,
        Error,
        Warning,
        NoData
    }
}