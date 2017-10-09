namespace Web.Models
{
    public class UploadDownloadModel
    {
        public string Company { get; set; }
        public string UploadedDownloadedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SecurityLevel { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string LastAccessDate{ get; set; }
        public string TotalSize { get; set; }
        public int FileCount { get; set; }
        public string Function { get; set; }
        public string SE { get; set; }
    }
}
