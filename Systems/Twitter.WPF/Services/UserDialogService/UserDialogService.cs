using System.Threading.Tasks;
using System.Windows;
using Twitter.WPF.Views;

namespace Twitter.WPF.Services.UserDialogService;

public class UserDialogService : IUserDialogService
{
    public void OpenMainWindow()
    {
        var dialog = new MainWindow();
        dialog.ShowDialog();
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