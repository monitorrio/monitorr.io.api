using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAuth0
{
    public class Identity
    {
        public string provider { get; set; }
        public string user_id { get; set; }
        public string connection { get; set; }
        public bool isSocial { get; set; }
    }
}
