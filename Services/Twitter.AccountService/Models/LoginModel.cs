namespace Twitter.AccountService.Models;

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

