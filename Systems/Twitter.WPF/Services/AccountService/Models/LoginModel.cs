namespace Twitter.WPF.Services.AccountService.Models;

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; } = Settings.ClientId;
    public string ClientSecret { get; set; } = Settings.ClientSecret;
}