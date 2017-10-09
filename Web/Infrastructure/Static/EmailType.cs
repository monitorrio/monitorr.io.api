namespace Web.Infrastructure.Static
{
    public enum EmailType
    {
        UploadConfirmation = 1,
        MessageToSender,
        DownloadConfirmation,
        FileRevoked,
        UserRegistered,
        WebPagePath,
        ImmanentFileDelete,
        FileDelete,
        ImmanentFileDeleteOwner,
        FileDeleteOwner,
        FordUserRegistration,
        SupplierUserRegistration,
        SupplierUserRegistration30Day,
        TempPassword,
        AdminFordUserRegistration,
        AdminSupplierUserRegistration,
        WelcomeFordUser,
        FordUserRejected,
        OutOfOffice,
        FordUserMigration,
        WelcomeSupplierUser,
        AdminSupplierLicenseIncrease,
        AdminSupplierUserRegistration30Day,
        SEFileReceivedEmail,
        SESendOnly,
        SEUsernamePassword,
        DSJobRequest,
        PasswordExpirationTenDay,
        InactivityNoticeThirty,
        InactivityNoticeTen,
        FileSent,
        PasswordReset
    }
}
