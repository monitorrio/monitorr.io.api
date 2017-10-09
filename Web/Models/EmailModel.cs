using Core.Domain;
using System.Collections.Generic;

namespace Web.Models
{
    public class EmailModel
    {
        public string FilesSentTo { get; set; }
        public string AllFilesSent { get; set; }
        public string NowDateTime { get; set; }
        public string ToEmail { get; set; }
        public int PassowrdWillExpireInDaysCount { get; set; }
        public string SenderCompanyName { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string ResetLink { get; set; }
        public string Domain { get; set; }
        public UserModel User { get; set; }
        public string AppUrl { get; set; }

        public EmailModel Map(UserModel user, IList<User> recipients)
        {
            ToEmail = user.Email;
            User = user;
            
            return this;
        }
        public EmailModel MapUser(UserModel user)
        {
            User = user;
            
            return this;
        }
    }
}
