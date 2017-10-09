using Web.Infrastructure.Extensions;

namespace Web.Infrastructure.Static
{
    public enum ResponseMessages
    {
        [DisplayText("Ok")]
        AllGood,
        [DisplayText("Ok")]
        Ok,
        [DisplayText("Please choose from an existing group, or cretae a new one.")]
        GroupMembersNoGroupSelected,
        [DisplayText("This group is no longer available.")]
        GroupMembersGroupUnavailable,
        [DisplayText("This user is already added to this group.")]
        GroupMembersMemberAlreadyAdded,
        [DisplayText("A group with the same name already exists. Please select a unique group.")]
        GroupAlreadyExists,
        [DisplayText("This notification list is no longer available.")]
        NotificationListNoListSelected,
        [DisplayText("A notification list with the same name already exists. Please select a unique notification list.")]
        NotificationListAlreadyAdded,
        [DisplayText("Please choose from an existing notification list, or cretae a new one.")]
        GroupMembersMembersNoListSelected,
        [DisplayText("This Email is already added to this notification list.")]
        NotificationListMemberAlreadyAdded,
        [DisplayText("User has been already added.")]
        FavoriteAlreadyAdded,
        [DisplayText("Could not locate file in the database.")]
        FileInfoNotFound,
        [DisplayText("We are sorry but it seems that this file no longer exist.")]
        FileNotFound,
        [DisplayText("An error has occurred with one of the uploaded files. Please upload the files again.")]
        ErrorWithUploadedFile,
        [DisplayText("Failed to update file.")]
        FileUpdateFailed,
        [DisplayText("Failed to delete file.")]
        FileDeleteFailed,
        [DisplayText("An error has ocurred downloading this file. Please try downloading this file again.")]
        FileMarkedAsDownloadedFailed,
        [DisplayText("Please specify a valid log name")]
        LogNameRequired,
        [DisplayText("A log with this name already exists.Please select a different name.")]
        LogNameDuplicate,
        [DisplayText("Log has been created.")]
        LogAdded,
        [DisplayText("Log has been updated.")]
        LogUpdated,
        [DisplayText("User has been created.")]
        UserAdded,
        [DisplayText("User cannot be empty.")]
        LogManageUserUserIdRequired,
        [DisplayText("Log cannot be empty.")]
        LogManageUserLogIdRequired,
        [DisplayText("Access has been changed.")]
        LogManageUserAccessChanged,
        [DisplayText("Email is required.")]
        InviteUserEmailRequired,
        [DisplayText("Email is already registered.")]
        EmailDuplicate,
        [DisplayText("The user is not found.")]
        UserNotFound,
        [DisplayText("The log is not found.")]
        LogNotFound,
        [DisplayText("The user cannot be invited.")]
        UserCannotBeInvited,
        [DisplayText("The log has been cleared.")]
        LogWasCleared,
        [DisplayText("The owner can clear log only.")]
        OnlyOwnerCanPerformClearing,
        [DisplayText("The owner can delete log only.")]
        OnlyOwnerCanPerformDeletion,
        [DisplayText("The owner can setup permissions only.")]
        OnlyOwnerCanPerformPermissionSettings,


    }
}
