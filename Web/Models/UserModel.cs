using System;
using Core;
using Core.Domain;
using SharpAuth0;
using static System.String;

namespace Web.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string DateCreated { get; set; }
        public string PictureUrl { get; set; }
        public string UserId { get; set; }

        public UserModel MapEntity(User x)
        {
            Id = x._id.ToString();
            FirstName = x.FirstName;
            LastName = x.LastName;
            Email = x.Email;
            DateCreated = x.DateCreated.ToShortDateString();
            IsActive = x.IsActive;
            Name = x.Name;
            return this;
        }

        public UserModel MapAuth0User(Claim auth0)
        {
            Id = auth0.UserId;
            Email = auth0.Email;
            PictureUrl = auth0.Picture;
            Name = auth0.Name;
            return this;
        }

        public User MapModel(User x, UserModel m)
        {
            x.FirstName = m.FirstName;
            x.LastName = m.LastName;
            return x;
        }

        public User ToEntity(UserModel m)
        {
            var x = new User();
            x.FirstName = m.FirstName;
            x.LastName = m.LastName;
            x.Email = m.Email;
            x.DateCreated =  IsNullOrEmpty(m.DateCreated) ? DateTime.UtcNow : DateTime.Parse(m.DateCreated);
            x.DateModified = AppTime.Now();
            x.IsActive = m.IsActive;
            x.PictureUrl = m.PictureUrl;
            return x;
        }
        public User ToNewLicensedUser(UserModel m)
        {
            var x = new User
            {
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email,
                DateCreated = IsNullOrEmpty(m.DateCreated) ? AppTime.Now() : DateTime.Parse(m.DateCreated),
                DateModified = AppTime.Now(),
                IsActive = true,
                IsDeleted = false,
                PictureUrl = m.PictureUrl,
                Name = m.Name,
                Auth0Id = m.UserId
            };

            return x;
        }
    }
}
