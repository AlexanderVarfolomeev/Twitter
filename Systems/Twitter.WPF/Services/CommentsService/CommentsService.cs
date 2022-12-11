using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Twitter.WPF.Services.AccountService;
using Twitter.WPF.Services.CommentsService.Models;
using Twitter.WPF.Services.UserDialogService;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Twitter.WPF.Services.CommentsService;

public class CommentsService : ICommentsService
{
    private readonly HttpClient _httpClient;
    private readonly IAccountService _accountService;
    private readonly IUserDialogService _userDialogService;

    public CommentsService(HttpClient httpClient, IAccountService accountService, IUserDialogService userDialogService)
    {
        _httpClient = httpClient;
        _accountService = accountService;
        _userDialogService = userDialogService;
    }

    public async Task<IEnumerable<CommentView>> GetCommentsByTweetId(string tweetId, int offset = 0, int limit = 10)
    {
        var getCommentsUri =
            $"{Settings.ApiRoot}/v1/Comments/get-comment-by-tweet?tweetId={tweetId}&offset={offset}&limit={limit}";
        var response = await _httpClient.GetAsync(getCommentsUri);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о комментариях.", "Ошибка при получении данных.");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<IEnumerable<CommentResponse>>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true}) ?? new List<CommentResponse>();

        var result = new List<CommentView>();
        foreach (var commentResponse in data)
        {
            var user = await _accountService.GetUser(commentResponse.CreatorId.ToString());
            var attachs = await GetAttachments(commentResponse.Id.ToString());
            result.Add(new CommentView(commentResponse) {Creator = user, Attachments = attachs.ToList()});
        }
        
        return result.OrderByDescending(x => x.CreationTime);
    }

    public async Task AddComment(string tweetId, CommentRequest comment, List<string> attachments)
    { 
        var addCommentUri = $"{Settings.ApiRoot}/v1/Comments?tweetId={tweetId}";
        var body = JsonSerializer.Serialize(comment);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(addCommentUri, request);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Ошибка при добавлении комментария.", "Ошибка.");
        }

        if (attachments.Count != 0)
        {
            var commentResponse = JsonSerializer.Deserialize<CommentResponse>(await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true})!;

            var addAttachmentsUri =
                $"{Settings.ApiRoot}/v1/TwitterFiles/add-base64Files-to-comment?commentId={commentResponse.Id.ToString()}";
            var attachmentsBody = JsonSerializer.Serialize(attachments);
            var attachmentsRequest = new StringContent(attachmentsBody, Encoding.UTF8, "application/json");
            var attachmentsResponse = await _httpClient.PostAsync(addAttachmentsUri, attachmentsRequest);

            if (!attachmentsResponse.IsSuccessStatusCode)
            {
                _userDialogService.ShowError("Ошибка при приклеплении данных", "Ошибка отправки данных."); 
            }
        }
    }

    private async Task<IEnumerable<string>> GetAttachments(string commentId)
    {
        var getCommentsAttachments = $"{Settings.ApiRoot}/v1/TwitterFiles/get-comment-files?commentId={commentId}";
        var response = await _httpClient.GetAsync(getCommentsAttachments);
        
        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о комментариях.", "Ошибка при получении данных.");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<IEnumerable<string>>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
        
        return data;
    }
}