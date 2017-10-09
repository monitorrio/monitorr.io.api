using System;
using Core;
using Core.Domain;
using SharpAuth0;
using Web.Infrastructure.Static;
using Web.Models;

namespace Web.Infrastructure.Extensions
{
    public static class UserExtensions
    {
        public static User UpdateFromModel(this User entity, UserModel model)
        {
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.DateModified = AppTime.Now();
            entity.IsActive = model.IsActive;
            entity.Name = model.Name;
            entity.PictureUrl = model.PictureUrl;
            entity.Auth0Id = model.UserId;
            entity.DateCreated = string.IsNullOrEmpty(model.DateCreated)
                ? AppTime.Now()
                : DateTime.Parse(model.DateCreated);

            return entity;
        }
        public static UserModel MapFreeUser(this Auth0User x)
        {
            var model = new UserModel
            {
                Email = x.email,
                FirstName = x.given_name,
                LastName = x.family_name,
                PictureUrl = x.picture
            };

            return model;
        }
    }
}