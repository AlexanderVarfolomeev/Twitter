using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Twitter.WPF.Services.CommentsService;
using Twitter.WPF.Services.CommentsService.Models;
using Twitter.WPF.Services.TweetsService;
using Twitter.WPF.Services.TweetsService.Models;
using Twitter.WPF.Services.UserDialogService;

namespace Twitter.WPF.ViewModels;

public partial class AddCommentViewModel : ObservableObject
{
    private readonly ICommentsService _commentsService;
    private readonly IUserDialogService _userDialogService;

    [ObservableProperty] private ObservableCollection<string> _images;
    [ObservableProperty] private string _text;

    public AddCommentViewModel(ICommentsService commentsService, IUserDialogService userDialogService)
    {
        _commentsService = commentsService;
        _userDialogService = userDialogService;

        Text = "comment";
        Images = new ObservableCollection<string>(new List<string>());
    }

    [RelayCommand]
    private void ChooseImage()
    {
        var image = _userDialogService.ChooseImageDialogWindow();
        if (image is not null)
            Images.Add(image);
    }

    [RelayCommand]
    private async Task SendComment()
    {
        await _commentsService.AddComment(ViewModelLocator.MainViewModel.SelectedTweet.Id,
            new CommentRequest() {Text = Text}, Images.ToList());
        ViewModelLocator.MainViewModel.CurrentTweetComments = new ObservableCollection<CommentView>(
            await _commentsService.GetCommentsByTweetId(ViewModelLocator.MainViewModel.SelectedTweet.Id));
    }
}