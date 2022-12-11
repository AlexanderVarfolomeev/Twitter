using System;
using System.Windows;
using Microsoft.VisualBasic;
using Twitter.WPF.Views;
using Microsoft.Win32;

namespace Twitter.WPF.Services.UserDialogService;

public class UserDialogService : IUserDialogService
{
    public void OpenMainWindow()
    {
        var dialog = new MainWindow();
        dialog.ShowDialog();
    }

    public void OpenCommentsWindow()
    {
        var dialog = new CommentsWindow();
        dialog.ShowDialog();
    }

    public void OpenAddTweetWindow()
    {
        var dialog = new AddTweetWindow();
        dialog.ShowDialog();
    }

    public void OpenAddCommentWindow()
    {
        var dialog = new AddCommentWindow();
        dialog.ShowDialog();
    }

    public void OpenSubscribersListWindow()
    {
        var dialog = new SubscribersListWindow();
        dialog.ShowDialog();
    }

    public void OpenSubscriptionsListWindow()
    {
        var dialog = new SubscriptionsListWindow();
        dialog.ShowDialog();
    }

    public string? ChooseImageDialogWindow()
    {
        var dialog = new OpenFileDialog();
        if (dialog.ShowDialog() == true)  
        {  
            string selectedFileName = dialog.FileName;
            byte[] image = System.IO.File.ReadAllBytes(dialog.FileName);
            return Convert.ToBase64String(image);
        }  
        return null;
    }
    


    public void ShowInformation(string information, string caption) => MessageBox.Show(information, caption, MessageBoxButton.OK, MessageBoxImage.Information);

    public void ShowWarning(string message, string caption) => MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);

    public void ShowError(string message, string caption) => MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);

    public bool Confirm(string message, string caption, bool exclamation = false) =>
        MessageBox.Show(
            message,
            caption,
            MessageBoxButton.YesNo,
            exclamation ? MessageBoxImage.Exclamation : MessageBoxImage.Question)
        == MessageBoxResult.Yes;
}