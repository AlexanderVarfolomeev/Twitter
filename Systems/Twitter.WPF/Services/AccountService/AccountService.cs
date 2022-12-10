using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Twitter.WPF.Services.AccountService.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Twitter.WPF.Services.AccountService;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public Task<AccountModel> RegisterUser(RegisterModel requestModel)
    {
        throw new System.NotImplementedException();
    }

    public async Task<LoginResult> LoginUser(LoginModel model)
    {
        var uri = $"{Settings.ApiRoot}/v1/TwitterAccount/login";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(uri, request);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(); // TODO окошко
        }

        var content = await response.Content.ReadAsStringAsync();
        var loginResult = JsonSerializer.Deserialize<LoginResult>(content) ?? new LoginResult();
        loginResult.Successful = response.IsSuccessStatusCode;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.AccessToken);
        
        JwtSecurityToken jwt = new JwtSecurityToken(loginResult.AccessToken);
        loginResult.Id =  jwt.Subject;
        return loginResult;
    }
}
