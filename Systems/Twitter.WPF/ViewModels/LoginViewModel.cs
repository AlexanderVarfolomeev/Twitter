using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Twitter.WPF.Services.AccountService;
using Twitter.WPF.Services.AccountService.Models;
using Twitter.WPF.Services.UserDialogService;

namespace Twitter.WPF.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAccountService _accountService;
    private readonly IUserDialogService _userDialogService;

    public LoginViewModel(IAccountService accountService, IUserDialogService userDialogService)
    {
        _accountService = accountService;
        _userDialogService = userDialogService;
        
        Email = "test@mail.ru";
        Password = "testtest";
    }

    [ObservableProperty] private string _email;
    [ObservableProperty] private string _password;

    public string CurrentUserId { get; private set; }
    
    [RelayCommand]
    private async Task Login()
    {
        var result = await _accountService.LoginUser(new LoginModel()
            {
                ClientId = Settings.ClientId,
                ClientSecret = Settings.ClientSecret, 
                Email = Email,
                Password = Password
            });

        if (result.Successful)
        {
            CurrentUserId = result.Id;
            await ViewModelLocator.MainViewModel.RefreshInfo();
            _userDialogService.OpenMainWindow();
        }
        else
        {
            _userDialogService.ShowError("Неверные логин или пароль!", "Ошибка идентификации.");
        }
    }
}