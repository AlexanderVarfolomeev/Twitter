using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.Client;
using Twitter.WPF.Services.AccountService;
using Twitter.WPF.Services.AccountService.Models;

namespace Twitter.WPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IAccountService _service;

    public MainViewModel(IAccountService service)
    {
        _service = service;
    }
    

    
}