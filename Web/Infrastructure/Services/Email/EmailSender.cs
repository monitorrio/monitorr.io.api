using System.Collections.Generic;
using System.Configuration;
using Web.Models;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace Web.Infrastructure.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public async Task SendAsync(SendEmailModel sendEmailModel)
        {
            var from = sendEmailModel.FromEmail;

            if (string.IsNullOrWhiteSpace(from))
            {
                from = ConfigurationManager.AppSettings["aws:fromEmail"];
            }

            var destination = new Destination
            {
                ToAddresses = sendEmailModel.Emails,
                BccAddresses = sendEmailModel.BccEmails
            };
            Content subject = new Content(sendEmailModel.Subject);
            Content textBody = new Content(sendEmailModel.Body);
            Body body = new Body
            {
                Html = textBody
            };
            var message = new Amazon.SimpleEmail.Model.Message(subject, body);

            SendEmailRequest request = new SendEmailRequest(from, destination, message);

            if (!string.IsNullOrWhiteSpace(sendEmailModel.ReplyTo))
            {
                request.ReplyToAddresses = new List<string>();
                request.ReplyToAddresses.Add(sendEmailModel.ReplyTo);
            }
            var clientId = ConfigurationManager.AppSettings["aws:ClientId"];
            var secretKey = ConfigurationManager.AppSettings["aws:SecretKey"];
            var sesRegion = ConfigurationManager.AppSettings["aws:SesRegion"];

            AWSCredentials credentials = new BasicAWSCredentials(clientId,
               secretKey);

            RegionEndpoint region = RegionEndpoint.GetBySystemName(sesRegion);

            AmazonSimpleEmailServiceClient client = new AmazonSimpleEmailServiceClient(credentials, region);

            await client.SendEmailAsync(request);
        }
    }

   

    public interface IEmailSender
    {
        Task SendAsync(SendEmailModel sendEmailModel);
    }
}