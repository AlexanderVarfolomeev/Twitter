using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Twitter.WPF.Services.AccountService;
using Twitter.WPF.Services.MessageService.Models;
using Twitter.WPF.Services.TweetsService.Models;
using Twitter.WPF.Services.UserDialogService;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace Twitter.WPF.Services.MessageService;

public class MessageService : IMessageService
{
    private readonly HttpClient _httpClient;
    private readonly IUserDialogService _userDialogService;
    private readonly IAccountService _accountService;

    public MessageService(HttpClient httpClient , IUserDialogService userDialogService, IAccountService accountService)
    {
        _httpClient = httpClient;
        _userDialogService = userDialogService;
        _accountService = accountService;
    }
    
    public async Task<IEnumerable<MessageView>> GetMessagesByUser(string userId, int offset = 0, int limit = 10)
    {
        var getTweetsUri = $"{Settings.ApiRoot}/v1/Message/get-by-user?userId={userId}&offset={offset}&limit={limit}";
        var response = await _httpClient.GetAsync(getTweetsUri);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о сообщениях.", "Ошибка при получении данных.");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<IEnumerable<MessageResponse>>(content, new JsonSerializerOptions {PropertyNameCaseInsensitive = true})!.ToList();

        List<MessageView> result = new List<MessageView>();
        foreach (var messageResponse in data)
        {
            MessageView msg = new MessageView()
            {
                CreationTime = messageResponse.CreationTime,
                ModificationTime = messageResponse.CreationTime,
                Id = messageResponse.Id,
                SenderId = messageResponse.SenderId,
                Text = messageResponse.Text,
                Sender = await _accountService.GetUser(messageResponse.SenderId.ToString())
            };
            result.Add(msg);
        }
        
        //TODO фотки

        return result.OrderByDescending(x => x.CreationTime);
    }
}