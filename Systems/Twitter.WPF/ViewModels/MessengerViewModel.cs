using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using Twitter.WPF.Services.AccountService;
using Twitter.WPF.Services.MessageService;
using Twitter.WPF.Services.MessageService.Models;
using Twitter.WPF.Services.UserDialogService;

namespace Twitter.WPF.ViewModels;

public partial class MessengerViewModel : ObservableObject
{
    private readonly IMessageService _messageService;
    private readonly IAccountService _accountService;
    private readonly IUserDialogService _userDialogService;

    private HubConnection _connection;
    
    [ObservableProperty] private ObservableCollection<MessageView> _messages;
    [ObservableProperty] private string _text;

    public MessengerViewModel(IMessageService messageService, IAccountService accountService, IUserDialogService userDialogService)
    {
        _messageService = messageService;
        _accountService = accountService;
        _userDialogService = userDialogService;
    }
    
    public async Task Connect()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5000/Chat",
                opt => opt.Headers.Add("Authorization", $"Bearer {Settings.AccessToken}"))
            .WithAutomaticReconnect()
            .Build();

        _connection.On<MessageResponse>("ReceiveMessage", async (response) =>
        {
            if (response.SenderId.ToString() == ViewModelLocator.MainViewModel.SelectedAccount.Id.ToString() || response.SenderId.ToString() == ViewModelLocator.MainViewModel.CurrentUser.Id.ToString())
            {
                MessageView msg = new MessageView()
                {
                    CreationTime = response.CreationTime,
                    ModificationTime = response.CreationTime,
                    Id = response.Id,
                    SenderId = response.SenderId,
                    Text = response.Text,
                    Sender = await _accountService.GetUser(response.SenderId.ToString())
                };
                Messages.Add(msg);
            }
        });

        Messages = new ObservableCollection<MessageView>(
            await _messageService.GetMessagesByUser(ViewModelLocator.MainViewModel.SelectedAccount.Id.ToString(), 0,
                1000));
        await _connection.StartAsync();
    }
    
    [RelayCommand]
    private async Task SendMessage()
    {
        MessageAdd msg = new MessageAdd(){Text = Text};
        
        await _connection.InvokeAsync("SendMessage", msg, ViewModelLocator.MainViewModel.SelectedAccount.Id.ToString());
    }
    
    

    /*[RelayCommand]
    private async Task Connect()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5000/Chat",
                opt => opt.Headers.Add("Authorization", $"Bearer {Settings.AccessToken}"))
            .WithAutomaticReconnect()
            .Build();

        _connection.On<string, string?>("ReceiveMessage", (message, senderUserName) =>
        {
            FriendUsername = senderUserName;
            RecevieMessages.Add(message);
        });

        await _connection.StartAsync();
    }

    [RelayCommand]
    private async Task SendMessage()
    {
        string ohotnikId = "46ae9ed9-62b9-4084-bcc5-fcf5799efbfe";
        string sexualizerId = "498073b5-a34e-4bca-801c-61ffa906528f";
        string cur = ViewModelLocator.MainViewModel.CurrentUser.Id.ToString() == ohotnikId ? ohotnikId : sexualizerId;
        string frend = cur == ohotnikId ? sexualizerId : ohotnikId;
        await _connection.InvokeAsync("SendMessage", cur, frend, TextToSend);
    }*/
}