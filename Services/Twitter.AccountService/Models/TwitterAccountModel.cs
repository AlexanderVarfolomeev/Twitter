using AutoMapper;
using Twitter.Entities.Users;

namespace Twitter.AccountService.Models;

public class TwitterAccountModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public DateTime Birthday { get; set; }
    public string PageDescription { get; set; } = string.Empty;
    public bool IsBanned { get; set; }

    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public Guid? AvatarId { get; set; }
}

public class TwitterAccountModelProfile : Profile
{
    public TwitterAccountModelProfile()
    {
        CreateMap<TwitterUser, TwitterAccountModel>();
    }
}