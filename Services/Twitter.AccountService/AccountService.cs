using Microsoft.AspNetCore.Identity;
using Twitter.AccountService.Models;
using Twitter.Entities.Users;

namespace Twitter.AccountService;

public class AccountService : IAccountService
{
    private readonly SignInManager<TwitterUser> _signInManager;
    private readonly UserManager<TwitterUser> _userManager;

    public AccountService(SignInManager<TwitterUser> signInManager, UserManager<TwitterUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    public Task<TwitterAccountModel> GetAccount()
    {
        throw new NotImplementedException();
    }

    public Task<TwitterAccountModel> GetAccounts()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RegisterAccount(TwitterAccountModel accountModel)
    {
        var user = new TwitterUser
        {
            Id = default,
            UserName = "Firstuser",
            NormalizedUserName = null,
            Email = "first@gmail.com",
            NormalizedEmail = null,
            EmailConfirmed = true,
            Name = "sasha",
            Surname = "varf",
            Birthday = default,
            PageDescription = "desc",
            IsBanned = false
        };

        var a = await _userManager.CreateAsync(user, accountModel.Password);

        return true;
    }

    public Task<bool> DeleteAccount()
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAccount()
    {
        throw new NotImplementedException();
    }
}