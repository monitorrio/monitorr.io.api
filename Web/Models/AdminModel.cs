using System.Collections.Generic;
namespace Web.Models
{
    public class AdminModel
    {
        public ICollection<UserModel> Users { get; set; }
    }
}
