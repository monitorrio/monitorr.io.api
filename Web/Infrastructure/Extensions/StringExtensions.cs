using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.String;

namespace Web.Infrastructure.Extensions
{
    public static class StringExtesnions
    {
        public static string Shorten(this string text, int length)
        {
            return
                !IsNullOrWhiteSpace(text) && text.Length >= length
                    ? text.Substring(0, length)
                    : text;
        }

        public static string GetRandomLetters(this string text, int length)
        {
            var random = new Random();
            var selectedChars = new StringBuilder();
            var normalizedText = text.DeleteSpecialCharacters();
            for (int i = 0; i < length; i++)
            {
                var index = random.Next(normalizedText.Length);
                selectedChars.Append(normalizedText[index]);
            }

            return selectedChars.ToString();
        }

        public static string DeleteSpecialCharacters(this string text)
        {
            return new string(text.Where(Char.IsLetter).ToArray());
        }
    }
}