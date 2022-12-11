using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Twitter.WPF.Services.TweetsService;
using Twitter.WPF.Services.TweetsService.Models;
using Twitter.WPF.Services.UserDialogService;
namespace Twitter.WPF.ViewModels;

public partial class AddTweetViewModel : ObservableObject
{
    private readonly ITweetsService _tweetsService;
    private readonly IUserDialogService _userDialogService;
    
    [ObservableProperty] private ObservableCollection<string> _images;
    [ObservableProperty] private string _text;

    public AddTweetViewModel(ITweetsService tweetsService, IUserDialogService userDialogService)
    {
        _tweetsService = tweetsService;
        _userDialogService = userDialogService;

        Text = "tweet";
        Images = new ObservableCollection<string>(new List<string>());
    }

    [RelayCommand]
    private void ChooseImage()
    {
        var image = _userDialogService.ChooseImageDialogWindow();
        if(image is not null)
            Images.Add(image);
    }
    
    [RelayCommand]
    private async Task SendTweet()
    {
        await _tweetsService.AddTweet(new TweetRequest() {Text = Text}, Images.ToList());
    }
}