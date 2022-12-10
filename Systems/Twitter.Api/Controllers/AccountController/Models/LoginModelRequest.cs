using AutoMapper;
using Twitter.AccountService.Models;

namespace Twitter.Api.Controllers.AccountController.Models;

public class LoginModelRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

public class LoginModelRequestProfile : Profile
{
    public LoginModelRequestProfile()
    {
        CreateMap<LoginModelRequest, LoginModel>();
    }
}