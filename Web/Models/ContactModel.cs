using Core.Domain;

namespace Web.Models
{
    public class ContactModel
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsGroupMember { get; set; }
        public bool IsNotificationListMember { get; set; }

        public ContactModel MapEntity(User x)
        {
            UserName = x.Email;
            FirstName = x.FirstName;
            LastName = x.LastName;
            Email = x.Email;
   
            return this;
        }
    }
}
