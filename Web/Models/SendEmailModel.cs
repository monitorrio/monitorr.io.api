using System.Collections.Generic;

namespace Web.Models
{
    public class SendEmailModel
    {
        private List<string> _emails;
        public List<string> Emails
        {
            get
            {
                _emails = _emails ?? new List<string>();
                return _emails;
            }
        }
        public List<string> BccEmails { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FromEmail { get; set; }
        public string ReplyTo { get; set; }

        public SendEmailModel AddRecipient(string email)
        {
            Emails.Add(email);
            return this;
        }
    }
}