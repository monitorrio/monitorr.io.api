using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Core.Configuration;
using Core.Domain;
using Hangfire;
using Web.Infrastructure.EmailTemplates;
using Web.Infrastructure.Repositories;
using Web.Infrastructure.Services.Email;
using Web.Models;

namespace Web.Infrastructure.Services
{
    public class JobRunner : IJobRunner
    {
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IErrorRepository _errorRepository;
        private readonly IEmailTemplateRenderer _emailTemplateRenderer;

        private const string DigestJobIdPrefix = "DigestJob";
        private TimeSpan _digestStart;
        private readonly INotificationRepository _notificationRepository;

        public JobRunner(ILogRepository logRepository,
            IUserRepository userRepository,
            IEmailSender emailSender,
            IErrorRepository errorRepository,
            INotificationRepository notificationRepository,
            IEmailTemplateRenderer emailTemplateRenderer)
        {
            _logRepository = logRepository;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _errorRepository = errorRepository;
            _notificationRepository = notificationRepository;
            _emailTemplateRenderer = emailTemplateRenderer;

            var digestStartConfig = AppDeployment.Instance.AppSetting(ConfigurationManager.AppSettings["DailyDigestEmailTime"], "00:00");

            if (!TimeSpan.TryParseExact(digestStartConfig, @"hh\:mm", CultureInfo.InvariantCulture, out _digestStart))
            {
                throw new Exception("Cannot parse start time for digest email task.");
            }
        }

        private void CreateRecurringJob(string jobId, Expression<Action> action)
        {
            RecurringJob.AddOrUpdate(jobId, action, Cron.Daily(_digestStart.Hours, _digestStart.Minutes), TimeZoneInfo.Utc);
        }

        public Expression<Action> DigestEmailJobAction(User user)
        {
            return () => SendDigestEmail(user.Email, user._id.ToString());
        }

        public void CreateDigestEmailJobIfNotExists(Notification notification)
        {
            var user = _userRepository.GetById(notification.UserId).Result;
            var jobId = GetDigestEmailJobId(notification);
            CreateRecurringJob(jobId, DigestEmailJobAction(user));
        }

        public void DeleteDigestEmailJobIfExists(Notification notification)
        {
            var jobId = GetDigestEmailJobId(notification);
            RecurringJob.RemoveIfExists(jobId);
        }

        public void DeleteDigestEmailJobIfExists(string userId)
        {
            var jobId = GetDigestEmailJobId(userId);
            RecurringJob.RemoveIfExists(jobId);
        }

        public void SendDigestEmail(string email, string userId)
        {
            var sendAsOneEmail = false;
            var digestEmailModels = new List<DigestEmailModel>();
            var logIds = _notificationRepository.FindAllDigestAsync(userId).Result;
            var logs = _logRepository.FindByLogIdAsync(logIds).Result;

            var startDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)).Date;
            var endDate = startDate.AddDays(1).AddTicks(-1);
            foreach (var log in logs)
            {
                var lastDayErrors = _errorRepository.GetForPeriodAsync(log.LogId, startDate, endDate).Result;

                var errorsGroupedByMessage = lastDayErrors.GroupBy(e => e.Message);
                var uniqueCount = errorsGroupedByMessage.Count();

                var criticalCount = lastDayErrors.Count(e => e.Severity == Severity.Critical);

                digestEmailModels.Add(new DigestEmailModel
                {
                    LogId = log.LogId,
                    LogName = log.Name,
                    Total = lastDayErrors.Count,
                    Unique = uniqueCount,
                    Critical = criticalCount,
                    Date = DateTime.UtcNow.ToString("MM'/'dd'/'yyyy")
                });
            }

            if (sendAsOneEmail)
            {
                SendDigestAsOneEmail(email, digestEmailModels);
            }
            else
            {
                SendDigestEmailSeparately(email, digestEmailModels);
            }
        }

        private void SendDigestEmailSeparately(string email, List<DigestEmailModel> digestEmailModels)
        {
            foreach (var digestEmailModel in digestEmailModels)
            {
                var subject = $"Daily Digest {digestEmailModel.Date} - {digestEmailModel.LogName}";
                var sendEmailModel = CreateDigestEmailSenderModel(digestEmailModel, email, subject);
                _emailSender.SendAsync(sendEmailModel);
            }
        }

        private void SendDigestAsOneEmail(string email, List<DigestEmailModel> digestEmailModels)
        {
            var firstDigestEmail = digestEmailModels.FirstOrDefault();
            if (firstDigestEmail == null)
            {
                throw new NullReferenceException("digestEmailModels is empty.");
            }
            var subject = $"Daily Digest {firstDigestEmail.Date}";
            var sendEmailModel = CreateDigestEmailSenderModel(digestEmailModels, email, subject);
            _emailSender.SendAsync(sendEmailModel);
        }

        private SendEmailModel CreateEmailSenderModel(object digestEmailModel, string email, string subject)
        {
            var body = _emailTemplateRenderer.Render(EmailTemplateName.DailyDigest, digestEmailModel);

            return new SendEmailModel
            {
                Subject = subject,
                Emails = { email },
                Body = body
            };
        }
        private SendEmailModel CreateDigestEmailSenderModel(IList<DigestEmailModel> emailModel,
            string email, string subject)
        {
            return CreateEmailSenderModel(emailModel, email, subject);
        }

        private SendEmailModel CreateDigestEmailSenderModel(DigestEmailModel digestEmailModel, string email, string subject)
        {
            var list = new List<DigestEmailModel>()
           {
               digestEmailModel
           };
            return CreateEmailSenderModel(list, email, subject);
        }

        private string GetDigestEmailJobId(Notification notification)
        {
            return GetDigestEmailJobId(notification.UserId);
        }

        private string GetDigestEmailJobId(string userId)
        {
            return $"{DigestJobIdPrefix}-{userId}";
        }
    }

    public interface IJobRunner
    {
        void DeleteDigestEmailJobIfExists(Notification x);
        void SendDigestEmail(string email, string userId);
        void CreateDigestEmailJobIfNotExists(Notification notification);
        void DeleteDigestEmailJobIfExists(string currentUserId);
    }
}