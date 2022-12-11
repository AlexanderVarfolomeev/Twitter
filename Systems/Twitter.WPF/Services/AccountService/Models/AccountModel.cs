using System;

namespace Twitter.WPF.Services.AccountService.Models;

public class AccountModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public DateTime Birthday { get; set; }
    public string PageDescription { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string AvatarBase64 { get; set; } = String.Empty;
}