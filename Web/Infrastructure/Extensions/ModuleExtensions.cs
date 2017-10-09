using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Web.Infrastructure.Extensions
{
    public static class ModuleExtensions
    {
        public enum ModuleType
        {
            //Shared
            Navigation,
            Notifications,
            Disclaimer,
            AlertPlaceholder,
            GoogleAnalytics,
            Aside,
            UserInfo,
            KnockoutReferences,
            AddLogModal,
            SocialButtons,
            LiveFeedWidget,
            LogWidget,
            LogsWidget
        }

        public static void RenderModule(this HtmlHelper htmlHelper, ModuleType type)
        {
            switch (type)
            {
                case ModuleType.Navigation:
                    htmlHelper.RenderAction("Navigation", "Module", new { id = 0, area = "Modules" });
                    break;
                case ModuleType.Notifications:
                    htmlHelper.RenderAction("Notifications", "Module", new { id = 0, area = "Modules" });
                    break;
                case ModuleType.Disclaimer:
                    htmlHelper.RenderAction("Disclaimer", "Module", new { area = "Modules" });
                    break;
                case ModuleType.AlertPlaceholder:
                    htmlHelper.RenderAction("AlertPlaceHolder", "Module", new { area = "Modules" });
                    break;
                case ModuleType.GoogleAnalytics:
                    htmlHelper.RenderAction("GoogleAnalytics", "Module", new { area = "Modules" });
                    break;
                case ModuleType.Aside:
                    htmlHelper.RenderAction("Aside", "Module", new { area = "Modules" });
                    break;
                case ModuleType.UserInfo:
                    htmlHelper.RenderAction("UserInfo", "Module", new { area = "Modules" });
                    break;
                case ModuleType.KnockoutReferences:
                    htmlHelper.RenderAction("KnockoutReferences", "Module", new { area = "Modules" });
                    break;
                case ModuleType.AddLogModal:
                    htmlHelper.RenderAction("AddLogModal", "Module", new { area = "Modules" });
                    break;
                case ModuleType.LiveFeedWidget:
                    htmlHelper.RenderAction("LiveFeedWidget", "Module", new { area = "Modules" });
                    break;
                case ModuleType.SocialButtons:
                    htmlHelper.RenderAction("SocialButtons", "Module", new { area = "Modules" });
                    break;
                case ModuleType.LogWidget:
                    htmlHelper.RenderAction("LogWidget", "Module", new { area = "Modules" });
                    break;
                case ModuleType.LogsWidget:
                    htmlHelper.RenderAction("RenderLogs", "Module", new { area = "Modules" });
                    break;
            }
        }
    }
}