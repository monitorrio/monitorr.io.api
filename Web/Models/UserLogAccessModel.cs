using System.Collections;
using System.Collections.Generic;
using Core.Domain;

namespace Web.Models
{
    public class UserLogAccessModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public LogAccessModel LogAccess { get; set; }

        public UserLogAccessModel MapEntity(User x)
        {
            Id = x._id.ToString();
            Email = x.Email;
            Name = x.Name;
            IsActive = x.IsActive;
            return this;
        }
    }
}