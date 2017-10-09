namespace Web.Models
{
    public class InviteUserModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string LogId { get; set; }

        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
    }
}