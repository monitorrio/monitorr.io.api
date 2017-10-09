using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAuth0
{
    public class Auth0User
    {
        public string email { get; set; }
        public bool email_verified { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string picture { get; set; }
        public string locale { get; set; }
        public string clientID { get; set; }
        public string updated_at { get; set; }
        public string user_id { get; set; }
        public string nickname { get; set; }
        public IList<Identity> identities { get; set; }
        public DateTime created_at { get; set; }
        public string global_client_id { get; set; }
    }
}
