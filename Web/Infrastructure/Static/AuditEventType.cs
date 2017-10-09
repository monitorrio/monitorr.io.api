using Web.Infrastructure.Extensions;

namespace Web.Infrastructure.Static
{
    public enum AuditEventType
    {
        [DisplayText("Successful Login")]
        SuccessfulLogin,
        [DisplayText("User Logout")]
        UserLogout,
        [DisplayText("Locked Out")]
        UserLockedOut,
        [DisplayText("Invalid Login Attempt.")]
        InvalidLoginAttempt,
        [DisplayText("User Login Failure")]
        LoginFailure,
        [DisplayText("User Requires Verification")]
        RequiresVerification,
        [DisplayText("User Information Changed")]
        UserInformationChanged,
        [DisplayText("Confirm Email")]
        ConfirmEmail,
        [DisplayText("User Verification")]
        Verification,
        [DisplayText("Successfuly Verified Secret Question")]
        SecretQuestionVerified,
        [DisplayText("Incorrect Answer When Verifying Secret Question")]
        SecretQuestionIncorrectAnswer,
        [DisplayText("Secret Question Reset")]
        SecretQuestionReset,
        [DisplayText("Secret Question Changed")]
        SecretQuestionChanged,
        [DisplayText("Password Reset Success")]
        PasswordResetSuccess,
        [DisplayText("Password Reset Failed")]
        PasswordResetFailed,
        [DisplayText("Password Changed")]
        PasswordChanged,
        [DisplayText("Registered Supplier")]
        SupplierRegistered,
        [DisplayText("Registered User")]
        RegisteredUser,
        [DisplayText("Updated User Profile")]
        UpdatedUserProfile,
        [DisplayText("Updated Company Profile")]
        UpdatedCompanyProfile,
        [DisplayText("Company Updated")]
        CompanyUpdated,
        [DisplayText("Requested Supplier License Increase")]
        RequestedSupplierLicenseIncrease,
        [DisplayText("Updated Supplier License Count")]
        UpdatedSupplierLicenseCount,
        [DisplayText("Deleted Supplier License")]
        DeletedSupplierLicense,
        [DisplayText("Added Favorite User")]
        AddedFavoriteUser,
        [DisplayText("Deleted Files")]
        DeletedFiles,
        [DisplayText("Deleted File")]
        DeletedFile,
        [DisplayText("Sent Files")]
        SentFiles,
        [DisplayText("Sent File")]
        SentFile,
        [DisplayText("Updated File")]
        UpdatedFile,
        [DisplayText("Locked File")]
        LockedFile,
        [DisplayText("Added Group Member")]
        AddedGroupMember,
        [DisplayText("Deleted Group Member")]
        DeletedGroupMember,
        [DisplayText("Added Group")]
        AddedGroup,
        [DisplayText("Updated Group Group Type")]
        UpdatedGroupGroupType,
        [DisplayText("Deleted Group")]
        DeletedGroup,
        [DisplayText("Added Notification List Member")]
        AddedNotificationListMember,
        [DisplayText("Deleted Notification List Member")]
        DeletedNotificationLisMember,
        [DisplayText("Deleted Notification List")]
        DeletedNotificationList,
        [DisplayText("Added Notification List")]
        AddedNotificationList,
        [DisplayText("Downloaded Package")]
        DownloadedPackage,
        [DisplayText("Downloaded File As Part Of Package")]
        DownloadedFileAsPartOfPackage,
        [DisplayText("Updated User Active Status")]
        UpdatedUserActiveStatus,
        [DisplayText("Updated User Deleted Status")]
        UpdatedUserDeletedStatus,
        [DisplayText("Generated Csv File")]
        GeneratedCsvFile,
    }
}
