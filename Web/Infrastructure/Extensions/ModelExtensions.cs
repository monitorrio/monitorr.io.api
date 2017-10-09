using Core;
using System;
using System.Collections.Generic;
using Web.Models;
using System.Linq;
using Core.Domain;
using Core.Configuration;
using Web.Infrastructure.Repositories;
using Web.Infrastructure.Static;

namespace Web.Infrastructure.Extensions
{
    public static class ModelExtensions
    {
        public static EmailModel ThenModel(this EmailModel model)
        {
            model.ToEmail = model.User.Email;
            model.AppUrl = AppDeployment.Instance.AppSetting("Domain", "");

            return model;
        }
        public static List<LogModel> ResolveErrorCount(this List<LogModel> items, IErrorRepository errorRepository)
        {
            foreach (var m in items)
            {
                m.ErrorCount = errorRepository.CountByLogId(m.LogId).ToString();
                m.CriticalCount = errorRepository.CountCriticalByLogIdAsync(m.LogId).Result;
            }

            return items;
        }

        public static List<LogModel> GetNotifications(this List<LogModel> items, INotificationRepository notificationRepository, string userId)
        {
            foreach (var m in items)
            {
                m.IsDailyDigestEmail = notificationRepository.IsUserSubscribedToDigestAsync(userId, m.LogId).Result;
                m.IsNewErrorEmail = notificationRepository.IsUserSubscribedToNewErrorAsync(userId, m.LogId).Result;
            }

            return items;
        }

        

        public static List<LogModel> GetSpaceUsed(this List<LogModel> items, IErrorRepository errorRepository)
        {
            var maxAvailabelSpace = GetMaxSpaceAvailable();
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var now = DateTime.Now;
            foreach (var logModel in items)
            {
                var currentCount =  errorRepository.CountForPeriodAsync(logModel.LogId, startOfMonth, now).Result;

                logModel.SpaceUsedPercentage =currentCount*100/maxAvailabelSpace;
            }

            return items;
        }

        private static int GetMaxSpaceAvailable()
        {
            var currentSecurityLevel = SecurityLevel.CurrentSecurityLevel();
            switch (currentSecurityLevel)
            {
                case eSecurityLevel.Free:
                    return 1000;
                case eSecurityLevel.Pro:
                    return 2000;
                case eSecurityLevel.Enterprise:
                    return 3000;
            }

            throw new Exception("Cannot get licence for user");
        }


        public static List<LogModel> ToToday(this List<LogModel> items, IErrorRepository errorRepository)
        {
            foreach (var m in items)
            {
                m.Errors = new List<ErrorModel>();
                m.Errors =
                    errorRepository.FindByLogId(m.LogId, AppTime.Now().StartOfDay().AddDays(-1), AppTime.Now().EndOfDay(), null)
                        .Select(x => new ErrorModel().MapEntity(x))
                        .ToList();

                m.LatestErrorCount = m.Errors.Count;
            }

            return items;
        }
        public static LogModel ResolveErrorCount(this LogModel item, IErrorRepository errorRepository)
        {
            item.ErrorCount = errorRepository.CountByLogId(item.LogId).ToString();
            return item;
        }

        public static string ResolveUsername(this UserModel model)
        {
            return (model.FirstName.ToCharArray().First() + model.LastName).ToLower();
        }

        public static List<ErrorModel> ResolveLogNames(this List<ErrorModel> items, List<Log> logs)
        {
            foreach (var item in items)
            {
                item.Log.Name = logs.First(x => x.LogId == item.Log.LogId).Name;
            }

            return items;
        }
    }
}
