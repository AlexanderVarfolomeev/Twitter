namespace Shared.Enum;

[Flags]
public enum TwitterPermissions
{
    None = 0x00,
    User = 0x01, // стандартные права юзера
    Admin = 0x02, // может банить аккаунты юзеров, удалять комментарии, твиты
    FullAccessAdmin = 0x04 // может делать все, в том числе удалять аккаунты, назначать права админа юзерам и тд
}