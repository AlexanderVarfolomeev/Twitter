using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Twitter.WPF.Services.AccountService.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Twitter.WPF.Services.TweetsService.Models;
using Twitter.WPF.Services.UserDialogService;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Twitter.WPF.Services.AccountService;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;
    private readonly IUserDialogService _userDialogService;

    public AccountService(HttpClient httpClient, IUserDialogService userDialogService)
    {
        _httpClient = httpClient;
        _userDialogService = userDialogService;
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
            _userDialogService.ShowError("Неверные логин или пароль.", "Не удалось войти!");
        }

        var content = await response.Content.ReadAsStringAsync();
        var loginResult = JsonSerializer.Deserialize<LoginResult>(content) ?? new LoginResult();
        loginResult.Successful = response.IsSuccessStatusCode;

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", loginResult.AccessToken);

        JwtSecurityToken jwt = new JwtSecurityToken(loginResult.AccessToken);
        loginResult.Id = jwt.Subject;
        return loginResult;
    }

    public async Task<IEnumerable<AccountModel>> GetUsers(int offset = 0, int limit = 10)
    {
        var uri = $"{Settings.ApiRoot}/v1/TwitterAccount?offset={offset}&limit={limit}";
        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о юзерах.", "Ошибка получения данных!");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<IEnumerable<AccountModel>>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true})!.ToList();

        foreach (var user in data)
        {
            user.AvatarBase64 = await GetAvatarBase64(user.Id.ToString());
        }

        return data;
    }

    public async Task<AccountModel> GetUser(string userId)
    {
        var uri = $"{Settings.ApiRoot}/v1/TwitterAccount/{userId}";

        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о юзерах.", "Ошибка получения данных!");
        }

        var content = await response.Content.ReadAsStringAsync();

        var user = JsonSerializer.Deserialize<AccountModel>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true}) ?? new AccountModel();

        user.AvatarBase64 = await GetAvatarBase64(userId);
        return user;
    }

    public async Task AddAvatar(string imageBase64)
    {
        var addAvatarUri = $"{Settings.ApiRoot}/v1/TwitterFiles/add-base64Avatar";
        var attachmentsBody = JsonSerializer.Serialize(imageBase64);
        var attachmentsRequest = new StringContent(attachmentsBody, Encoding.UTF8, "application/json");
        var attachmentsResponse = await _httpClient.PostAsync(addAvatarUri, attachmentsRequest);

        if (!attachmentsResponse.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Ошибка при приклеплении данных", "Ошибка отправки данных.");
        }
    }

    public async Task<IEnumerable<AccountModel>> GetSubscribers(string userId, int offset = 0, int limit = 10)
    { 
        var uri = $"{Settings.ApiRoot}/v1/TwitterAccount/get-subscribers-{userId}?offset={offset}&limit={limit}";
        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о юзерах.", "Ошибка получения данных!");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<IEnumerable<AccountModel>>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true})!.ToList();

        foreach (var user in data)
        {
            user.AvatarBase64 = await GetAvatarBase64(user.Id.ToString());
        }

        return data;
    }

    public async Task<IEnumerable<AccountModel>> GetSubscriptions(string userId, int offset = 0, int limit = 10)
    {
        var uri = $"{Settings.ApiRoot}/v1/TwitterAccount/get-subscriptions-{userId}?offset={offset}&limit={limit}";
        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о юзерах.", "Ошибка получения данных!");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<IEnumerable<AccountModel>>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true})!.ToList();

        foreach (var user in data)
        {
            user.AvatarBase64 = await GetAvatarBase64(user.Id.ToString());
        }

        return data;
    }

    private async Task<string> GetAvatarBase64(string userId)
    {
        var getAvatarUri = $"{Settings.ApiRoot}/v1/TwitterFiles/get-avatar?userId={userId}";
        var response = await _httpClient.GetAsync(getAvatarUri);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о юзерах.", "Ошибка получения данных!");
        }

        var content = await response.Content.ReadAsStringAsync();

        if (content == "")
            content = Settings.DefaultAvatar;
        return content;
    }
}