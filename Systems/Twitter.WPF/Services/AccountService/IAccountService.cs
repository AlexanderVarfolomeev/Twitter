using System.Threading.Tasks;
using IdentityModel.Client;
using Twitter.WPF.Services.AccountService.Models;

namespace Twitter.WPF.Services.AccountService;

public interface IAccountService
{
    Task<AccountModel> RegisterUser(RegisterModel requestModel);
    Task<LoginResult> LoginUser(LoginModel model);
}