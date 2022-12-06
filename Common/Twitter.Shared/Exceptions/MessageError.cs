namespace Shared.Exceptions;

public static class MessageError
{
    public const string NotFoundError = "Object not found.";
    public const string YouBannedError = "You are banned.";
    public const string AccessRightsError = "Not enough rights for this.";
    public const string IncorrectEmailOrPasswordError = "Incorrect email or password.";
    public const string OnlyAdminCanDoItError = "Only admin can do it.";
    public const string CantBanAdminError = "You can't ban the admin.";
    public const string OnlyAdminOrAccountOwnerCanDoIdError = "Only admin or account owner can do it.";
    public const string OnlyAccountOwnerCanDoIdError = "Only account owner can do it.";
    public const string UserWithThisEmailExistsError = "User with this email exists.";
}