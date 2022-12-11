using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel.Client;
using Twitter.WPF.Services.AccountService.Models;

namespace Twitter.WPF.Services.AccountService;

public interface IAccountService
{
    Task<AccountModel> RegisterUser(RegisterModel requestModel);
    Task<LoginResult> LoginUser(LoginModel model);
    Task<IEnumerable<AccountModel>> GetUsers(int offset = 0, int limit = 10);
    Task<AccountModel> GetUser(string userId);
    Task AddAvatar(string imageBase64);
    Task<IEnumerable<AccountModel>> GetSubscribers(string userId, int offset = 0, int limit = 10);
    Task<IEnumerable<AccountModel>> GetSubscriptions(string userId, int offset = 0, int limit = 10);
}