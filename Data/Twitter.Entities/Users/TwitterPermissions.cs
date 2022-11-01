namespace Twitter.Entities.Users;

[Flags]
public enum TwitterPermissions
{
    None = 0x00,
    User = 0x01,
    Admin = 0x02,
    FullAccessAdmin = 0x04
}