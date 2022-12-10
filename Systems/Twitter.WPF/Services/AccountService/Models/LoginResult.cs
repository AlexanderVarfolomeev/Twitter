using System;
using System.Text.Json.Serialization;

namespace Twitter.WPF.Services.AccountService.Models;

public class LoginResult
{
    public bool Successful { get; set; }
    
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("refreshToken")]
    public string? RefreshToken { get; set; }
    
    [JsonPropertyName("expiresIn")]
    public int? ExpiresIn { get; set; }

    [JsonPropertyName("tokenType")]
    public string? TokenType { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; set; }
    
    public string Id { get; set; } = null!;
}
