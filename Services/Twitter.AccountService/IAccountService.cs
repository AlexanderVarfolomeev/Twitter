using Twitter.AccountService.Models;

namespace Twitter.AccountService;

public interface IAccountService
{
    Task<TwitterAccountModel> GetAccount();
    Task<TwitterAccountModel> GetAccounts();
    Task<bool> RegisterAccount(TwitterAccountModel accountModel);
    Task<bool> DeleteAccount();
    Task<bool> UpdateAccount();
}