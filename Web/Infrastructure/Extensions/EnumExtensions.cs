using System;
using System.Reflection;

namespace Web.Infrastructure.Extensions
{
    public class DisplayText : Attribute
    {
        public DisplayText(string text)
        {
            Text = text;
        }
        public string Text { get; set; }
    }
    /// <summary>
    /// get a readable text from any enum decorated with [DisplayText("custom text")] attribute
    /// For example EmailTemplates.PasswordReset.ToDesc() will return "password-reset.html"
    /// </summary>
    public static class EnumExtensions
    {
        public static string ToDesc(this Enum en)
        {

            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {

                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DisplayText),false);

                if (attrs.Length > 0)

                    return ((DisplayText)attrs[0]).Text;

            }

            return en.ToString();
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
