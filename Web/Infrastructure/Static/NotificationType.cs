using Web.Infrastructure.Extensions;

namespace Web.Infrastructure.Static
{
    public enum NotificationType
    {
        [DisplayText("Activate Out of Office Message")]
        ActivateOutOfOfficeMessage,
        [DisplayText("Recive a file")]
        ReciveAFile,
        [DisplayText("Send a file")]
        SendAFile,
        [DisplayText("File about to expire")]
        FileAboutToExpire,
        [DisplayText("Recipient has downloaded your file")]
        RecipientHasDownloadedYourFile,
    }
}
