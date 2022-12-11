using Microsoft.Extensions.DependencyInjection;

namespace Twitter.WPF.ViewModels;

public static class ViewModelLocator
{
    public static MainViewModel MainViewModel = App.Host.Services.GetRequiredService<MainViewModel>();
    public static LoginViewModel LoginViewModel = App.Host.Services.GetRequiredService<LoginViewModel>();
    public static AddTweetViewModel AddTweetViewModel = App.Host.Services.GetRequiredService<AddTweetViewModel>();
    public static AddCommentViewModel AddCommentViewModel = App.Host.Services.GetRequiredService<AddCommentViewModel>();
}