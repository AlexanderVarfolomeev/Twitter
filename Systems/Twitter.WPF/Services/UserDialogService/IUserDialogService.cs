using System.Threading.Tasks;

namespace Twitter.WPF.Services.UserDialogService;

public interface IUserDialogService
{
    void OpenMainWindow();
    
    void ShowInformation(string information, string caption);

    void ShowWarning(string message, string caption);

    void ShowError(string message, string caption);

    bool Confirm(string message, string caption, bool exclamation = false);
}