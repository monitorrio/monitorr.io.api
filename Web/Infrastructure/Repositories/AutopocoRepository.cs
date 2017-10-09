using Core;
using System;
using AutoPoco;
using Web.Models;
using System.Linq;
using AutoPoco.Engine;
using AutoPoco.DataSources;
using System.Collections.Generic;
using Web.Infrastructure.Migrations.AutoPoco;

namespace Web.Infrastructure.Repositories
{
    public class AutopocoRepository : IAutopocoRepository
    {
        public IGenerationSession Session { get; set; }
        /// <summary>
        /// This is for generating test data
        /// </summary>
        public AutopocoRepository()
        {
            IGenerationSessionFactory factory = AutoPocoContainer.Configure(x =>
            {
                x.Conventions(c => c.UseDefaultConventions());
                x.AddFromAssemblyContainingType<UserModel>();
                x.Include<UserModel>().Setup(r => r.FirstName).Use<FirstNameSource>();
                x.Include<UserModel>().Setup(r => r.LastName).Use<LastNameSource>();
                x.Include<UserModel>().Setup(r => r.Email).Use<EmailAddressSource>();
            });
            Session = factory.CreateSession();
        }
        public List<UserModel> GenerateUsers(int count)
        {
            List<UserModel> users = Session.List<UserModel>(count).Get().ToList();
            foreach (var u in users)
            {
                //u.Username = (u.FirstName.ToCharArray().First() + u.LastName).ToLower() + AppTime.Now().Second + Guid.NewGuid().ToString().ToCharArray().First() + Guid.NewGuid().ToString().ToCharArray().Last();
                //u.Email = u.Username + u.Email;
            }
            return users;
        }

        public UserModel GenerateUser()
        {
            UserModel user = Session.Single<UserModel>().Get();

            return user;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
    public interface IAutopocoRepository
    {
        List<UserModel> GenerateUsers(int count);
        UserModel GenerateUser();
    }
}
