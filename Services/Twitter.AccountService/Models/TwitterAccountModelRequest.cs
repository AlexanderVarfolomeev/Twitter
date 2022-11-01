using AutoMapper;
using Twitter.Entities.Users;

namespace Twitter.AccountService.Models;
public class TwitterAccountModelRequest
{
    public string Name { get; set; } = String.Empty;
    public string Surname { get; set; } = String.Empty;
    public string? Patronymic { get; set; }
    public DateTime Birthday { get; set; }
    public string PageDescription { get; set; } = String.Empty;
    
    public string UserName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string PhoneNumber { get; set; } = String.Empty;
    
    public string Password { get; set; } = String.Empty;
}

public class TwitterAccountModelRequestProfile : Profile
{
    public TwitterAccountModelRequestProfile()
    {
        CreateMap<TwitterAccountModelRequest, TwitterUser>();
    }
}