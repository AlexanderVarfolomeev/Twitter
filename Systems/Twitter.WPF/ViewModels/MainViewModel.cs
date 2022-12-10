using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using Twitter.WPF.Services;

namespace Twitter.WPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private IService _service;
    [ObservableProperty] private string text;

    public MainViewModel(IService service)
    {
        _service = service;
        text = _service.GetStr();
    }
}