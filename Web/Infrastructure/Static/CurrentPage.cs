using System.Configuration;
using System.Web;

namespace Web.Infrastructure.Static
{
    public static class CurrentPage
    {
        public enum eCurrentPage
        {
            Undefined,
            FileManager,
            Home,
            Mailbox,
            Contacts,
            Admin,
            Company,
            Help
        }
        public static eCurrentPage Retrive()
        {
            string currentAction = HttpContext.Current.Request.Url.AbsolutePath.ToLower();

            if (currentAction.ToLower().Equals("/") || currentAction.Equals(ConfigurationManager.AppSettings["ApplicationName"].ToLower()))
            {
                return eCurrentPage.Home;
            }
            if (currentAction.Equals("/filemanager"))
            {
                return eCurrentPage.FileManager;
            }
            if (currentAction.Contains("/mailbox"))
            {
                return eCurrentPage.Mailbox;
            }
            if (currentAction.Contains("/contacts"))
            {
                return eCurrentPage.Contacts;
            }
            if (currentAction.Contains("/admin"))
            {
                return eCurrentPage.Admin;
            }
            if (currentAction.Contains("/company"))
            {
                return eCurrentPage.Company;
            }
            if (currentAction.Contains("/help"))
            {
                return eCurrentPage.Help;
            }

            return eCurrentPage.Undefined;
        }
    }
}
