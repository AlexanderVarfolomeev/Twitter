using Twitter.AccountService.Models;

namespace Twitter.AccountService;

public interface IAccountService
{
    Task<IEnumerable<TwitterAccountModel>> GetAccounts();
    Task<TwitterAccountModel> GetAccountById(Guid id);
    Task DeleteAccount(Guid id);
    Task<TwitterAccountModel> RegisterAccount(TwitterAccountModelRequest requestModel);
    Task<TwitterAccountModel> UpdateAccount(Guid id, TwitterAccountModelRequest requestModel);
}