using Microsoft.AspNetCore.Identity;

namespace Twitter.Entities.Auth;

public class TwitterUser : IdentityUser<Guid>
{
    public string Name { get; set; } = String.Empty;
    public string Surname { get; set; } = String.Empty;
    public string? Patronymic { get; set; }
    public DateTime Birthday { get; set; }
    public string PageDescription { get; set; } = String.Empty;
    public bool IsBanned { get; set; } = false;
    
    
    // Поля email, phoneNumber, id, passwordHash, userName (ник) определены в IdentityUser
    // TODO добавить аватар 
}