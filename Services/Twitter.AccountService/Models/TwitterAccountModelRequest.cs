using AutoMapper;
using Twitter.Entities.Users;

namespace Twitter.AccountService.Models;

public class TwitterAccountModelRequest
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public DateTime Birthday { get; set; }
    public string PageDescription { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public class TwitterAccountModelRequestProfile : Profile
{
    public TwitterAccountModelRequestProfile()
    {
        CreateMap<TwitterAccountModelRequest, TwitterUser>();
    }
}