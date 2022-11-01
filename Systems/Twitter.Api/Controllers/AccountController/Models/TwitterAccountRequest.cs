using AutoMapper;
using Twitter.AccountService.Models;

namespace Twitter.Api.Controllers.AccountController.Models;

public class TwitterAccountRequest
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

public class TwitterAccountRequestProfile : Profile
{
    public TwitterAccountRequestProfile()
    {
        CreateMap<TwitterAccountRequest, TwitterAccountModelRequest>();
    }
}