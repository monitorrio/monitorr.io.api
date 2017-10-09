using System.Collections.Generic;

namespace Web.Models
{
    public class NewErrorListModel
    {
        public long Total { get; set; }
        public IList<NewErrorModel> Errors { get; set; }
    }
}