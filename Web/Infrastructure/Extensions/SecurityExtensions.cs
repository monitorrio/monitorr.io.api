using System;
using System.Linq;

namespace Web.Infrastructure.Extensions
{
    public static class SecurityExtensions
    {
        public static string ToRole(this string property)
        {
            switch (property)
            {
                case "Supplier":
                    return "";
                default:
                    return null;

            }
        }
        public static int ToDefaultLicenseCount()
        {
            return 5;
        }
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
