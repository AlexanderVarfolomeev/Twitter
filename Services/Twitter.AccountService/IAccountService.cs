using IdentityModel.Client;
using Twitter.AccountService.Models;

namespace Twitter.AccountService;

public interface IAccountService
{
    IEnumerable<TwitterAccountModel> GetAccounts(int offset = 0, int limit = 10);
    IEnumerable<TwitterAccountModel> GetSubscribers(Guid userId, int offset = 0, int limit = 10);
    IEnumerable<TwitterAccountModel> GetSubscriptions(Guid userId, int offset = 0, int limit = 10);
    
    TwitterAccountModel GetAccountById(Guid id);
    void DeleteAccount(Guid id);
    TwitterAccountModel UpdateAccount(Guid id, TwitterAccountModelRequest requestModel);
    void Subscribe(Guid userId);
    void BanUser(Guid userId);
    Task<TwitterAccountModel> RegisterUser(TwitterAccountModelRequest requestModel);
    Task<TokenResponse> LoginUser(LoginModel model);
    
}