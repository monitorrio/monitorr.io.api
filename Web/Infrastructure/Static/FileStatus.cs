using Web.Infrastructure.Extensions;

namespace Web.Infrastructure.Static
{
    public enum FileStatus
    {
        [DisplayText("Downloaded")]
        IsDownloaded,
        [DisplayText("Not Downloaded")]
        IsNotDownloaded,
        [DisplayText("Downloaded, To Be Deleted")]
        IsDownloadedToBeDeleted,
        [DisplayText("Not Downloaded, To Be Deleted")]
        IsNotDownloadedToBeDeleted
    }
}
