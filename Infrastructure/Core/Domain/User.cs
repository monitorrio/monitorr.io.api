using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Domain
{
    public class User:MongoEntity
    {
        public string Auth0Id { get; set; }
        [BsonRequired]
        public string FirstName { get; set; }
        [BsonRequired]
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool RegistrationComplete { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string PictureUrl { get; set; }

        [Required]
        public string RoleId { get; set; }

        [BsonIgnore]
        public string FullName => FirstName + " " + LastName;

    }
}