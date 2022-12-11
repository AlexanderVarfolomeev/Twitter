using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Twitter.WPF.Services.UserDialogService;

public interface IUserDialogService
{
    void OpenMainWindow();
    void OpenCommentsWindow();
    void OpenAddTweetWindow();
    void OpenAddCommentWindow();
    void OpenSubscribersListWindow();
    void OpenSubscriptionsListWindow();
    string? ChooseImageDialogWindow();
    void ShowInformation(string information, string caption);

    void ShowWarning(string message, string caption);

    void ShowError(string message, string caption);

    bool Confirm(string message, string caption, bool exclamation = false);
}