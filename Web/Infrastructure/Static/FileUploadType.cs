using Web.Infrastructure.Extensions;

namespace Web.Infrastructure.Static
{
    public enum FileUploadType
    {
        [DisplayText("Send File")]
        SendFile,
        [DisplayText("Package")]
        Package,
        [DisplayText("SE File")]
        SEFile,
    }
}
