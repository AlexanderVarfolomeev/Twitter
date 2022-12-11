using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Twitter.WPF.Services.AccountService;
using Twitter.WPF.Services.AccountService.Models;
using Twitter.WPF.Services.CommentsService;
using Twitter.WPF.Services.CommentsService.Models;
using Twitter.WPF.Services.TweetsService;
using Twitter.WPF.Services.TweetsService.Models;
using Twitter.WPF.Services.UserDialogService;

namespace Twitter.WPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IAccountService _accountService;
    private readonly ITweetsService _tweetsService;
    private readonly IUserDialogService _userDialogService;
    private readonly ICommentsService _commentsService;

    public MainViewModel(IAccountService accountService, ITweetsService tweetsService,
        IUserDialogService userDialogService, ICommentsService commentsService)
    {
        _accountService = accountService;
        _tweetsService = tweetsService;
        _userDialogService = userDialogService;
        _commentsService = commentsService;

        CountOfSubscribers = 0;
        CountOfSubscriptions = 0;
    }

    [ObservableProperty] private AccountModel _currentUser;
    [ObservableProperty] private ObservableCollection<TweetView> _currentUserTweets;
    [ObservableProperty] private TweetView _selectedTweet;
    [ObservableProperty] private ObservableCollection<CommentView> _currentTweetComments;
    [ObservableProperty] private int _countOfSubscribers;
    [ObservableProperty] private int _countOfSubscriptions;
    [ObservableProperty] private ObservableCollection<AccountModel> _subscribers;
    [ObservableProperty] private ObservableCollection<AccountModel> _subscriptions;
    [ObservableProperty] private ObservableCollection<TweetView> _subscriptionsTweets;

    public async Task RefreshInfo()
    {
        CurrentUser = await _accountService.GetUser(ViewModelLocator.LoginViewModel.CurrentUserId);
        CurrentUserTweets =
            new ObservableCollection<TweetView>(
                await _tweetsService.GetTweetsByUserId(CurrentUser.Id.ToString(), 0, 1000));
        Subscribers =
            new ObservableCollection<AccountModel>(
                await _accountService.GetSubscribers(CurrentUser.Id.ToString(), 0, 10000));
        Subscriptions =
            new ObservableCollection<AccountModel>(
                await _accountService.GetSubscriptions(CurrentUser.Id.ToString(), 0, 10000));
        CountOfSubscriptions = Subscriptions.Count;
        CountOfSubscribers = Subscribers.Count;
        SubscriptionsTweets = new ObservableCollection<TweetView>(await _tweetsService.GetTweetsBySubscriptions(0, 100000));
        foreach (var tweet in CurrentUserTweets)
        {
            tweet.Creator = CurrentUser;
        }
    }

    [RelayCommand]
    private async Task OpenComments()
    {
        CurrentTweetComments =
            new ObservableCollection<CommentView>(await _commentsService.GetCommentsByTweetId(SelectedTweet.Id));
        _userDialogService.OpenCommentsWindow();
        await RefreshInfo();
    }

    [RelayCommand]
    private async Task AddTweet()
    {
        _userDialogService.OpenAddTweetWindow();
        await RefreshInfo();
    }

    [RelayCommand]
    private async Task AddComment()
    {
        _userDialogService.OpenAddCommentWindow();
        await RefreshInfo();
    }

    [RelayCommand]
    private async Task LikeTweet()
    {
        await _tweetsService.LikeTweet(SelectedTweet.Id);
        await RefreshInfo();
    }

    [RelayCommand]
    private async Task AddAvatar()
    {
        var image = _userDialogService.ChooseImageDialogWindow();
        if (image is not null)
        {
            await _accountService.AddAvatar(image);
            await RefreshInfo();
        }
    }

    [RelayCommand]
    private void ViewSubscribersList()
    {
        _userDialogService.OpenSubscribersListWindow();
    }  
    
    [RelayCommand]
    private void ViewSubscriptionsList()
    {
        _userDialogService.OpenSubscriptionsListWindow();
    }
}