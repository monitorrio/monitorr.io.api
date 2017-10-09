using Core.Configuration;
using Growl.Connector;

namespace Core
{
    public class GrowlHelper
    {
        public static void SimpleGrowl(string title, string message = "")
        {
            GrowlConnector simpleGrowl = new GrowlConnector();
            Application thisApp = new Application(AppDeployment.Instance.AppSetting("ApplicationName", "Oem Advantage"));
            NotificationType simpleGrowlType = new NotificationType("ERROR");
            simpleGrowl.Register(thisApp, new[] { simpleGrowlType });
            Notification myGrowl = new Notification(AppDeployment.Instance.AppSetting("ApplicationName", "Oem Advantage"), "ERROR", title, title, message);
            simpleGrowl.Notify(myGrowl);
          
        }
    }
}
