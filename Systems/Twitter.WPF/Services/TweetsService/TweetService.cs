using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Twitter.WPF.Services.AccountService;
using Twitter.WPF.Services.CommentsService.Models;
using Twitter.WPF.Services.TweetsService.Models;
using Twitter.WPF.Services.UserDialogService;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Twitter.WPF.Services.TweetsService;

public class TweetService : ITweetsService
{
    private readonly HttpClient _httpClient;
    private readonly IUserDialogService _userDialogService;
    private readonly IAccountService _accountService;

    public TweetService(HttpClient httpClient, IUserDialogService userDialogService, IAccountService accountService)
    {
        _httpClient = httpClient;
        _userDialogService = userDialogService;
        _accountService = accountService;
    }


    public async Task<IEnumerable<TweetView>> GetTweets(int offset = 0, int limit = 10)
    {
        var getTweetsUri = $"{Settings.ApiRoot}/v1/Tweets/get-tweets?offset={offset}&limit={limit}";


        var response = await _httpClient.GetAsync(getTweetsUri);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о твитах.", "Ошибка при получении данных.");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<IEnumerable<TweetResponse>>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true})!.ToList();

        var tweetsViews = new List<TweetView>();
        foreach (var tweet in data)
        {
            var likes = await GetCountOfLikes(tweet.Id);
            tweetsViews.Add(new TweetView()
            {
                CreatorId = tweet.CreatorId,
                CountOfLikes = likes,
                CountOfComments = await GetCountOfComments(tweet.Id),
                CreationTime = tweet.CreationTime,
                Creator = await _accountService.GetUser(tweet.CreatorId.ToString()),
                Attachments = (await GetAttachments(tweet.Id)).ToList(),
                Text = tweet.Text,
                Id = tweet.Id.ToString()
            });
        }

        return tweetsViews;
    }

    public async Task<IEnumerable<TweetView>> GetTweetsByUserId(string userId, int offset = 0, int limit = 10)
    {
        var getTweetsUri = $"{Settings.ApiRoot}/v1/Tweets/get-tweets-by-user:{userId}?offset={offset}&limit={limit}";
        var response = await _httpClient.GetAsync(getTweetsUri);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о твитах.", "Ошибка при получении данных.");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<IEnumerable<TweetResponse>>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true})!.ToList() ?? new List<TweetResponse>();

        var tweetsViews = new List<TweetView>();
        foreach (var tweet in data)
        {
            var likes = await GetCountOfLikes(tweet.Id);
            tweetsViews.Add(new TweetView()
            {
                CreatorId = tweet.CreatorId,
                CountOfLikes = likes,
                CountOfComments = await GetCountOfComments(tweet.Id),
                CreationTime = tweet.CreationTime,
                Creator = await _accountService.GetUser(tweet.CreatorId.ToString()),
                Attachments = (await GetAttachments(tweet.Id)).ToList(),
                Text = tweet.Text,
                Id = tweet.Id.ToString()
            });
        }

        return tweetsViews.OrderByDescending(x => x.CreationTime);
    }

    public async Task<IEnumerable<TweetView>> GetTweetsBySubscriptions(int offset = 0, int limit = 10)
    {
        var getTweetsUri = $"{Settings.ApiRoot}/v1/Tweets/get-tweets-subscription?offset={offset}&limit={limit}";

        var response = await _httpClient.GetAsync(getTweetsUri);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о твитах.", "Ошибка при получении данных.");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<IEnumerable<TweetResponse>>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true})!.ToList();

        var tweetsViews = new List<TweetView>();
        foreach (var tweet in data)
        {
            var likes = await GetCountOfLikes(tweet.Id);
            tweetsViews.Add(new TweetView()
            {
                CreatorId = tweet.CreatorId,
                CountOfLikes = likes,
                CountOfComments = await GetCountOfComments(tweet.Id),
                CreationTime = tweet.CreationTime,
                Creator = await _accountService.GetUser(tweet.CreatorId.ToString()),
                Attachments = (await GetAttachments(tweet.Id)).ToList(),
                Text = tweet.Text,
                Id = tweet.Id.ToString()
            });
        }

        return tweetsViews;
        throw new NotImplementedException();
    }

    public async Task AddTweet(TweetRequest tweet, List<string> attachments)
    {
        var addTweetUri = $"{Settings.ApiRoot}/v1/Tweets";
        var body = JsonSerializer.Serialize(tweet);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(addTweetUri, request);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Ошибка при добавлении твита.", "Ошибка.");
        }

        if (attachments.Count != 0)
        {
            var tweetResponse = JsonSerializer.Deserialize<TweetResponse>(await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true})!;

            var addAttachmentsUri =
                $"{Settings.ApiRoot}/v1/TwitterFiles/add-base64Files-to-tweet?tweetId={tweetResponse.Id.ToString()}";
            var attachmentsBody = JsonSerializer.Serialize(attachments);
            var attachmentsRequest = new StringContent(attachmentsBody, Encoding.UTF8, "application/json");
            var attachmentsResponse = await _httpClient.PostAsync(addAttachmentsUri, attachmentsRequest);

            if (!attachmentsResponse.IsSuccessStatusCode)
            {
                _userDialogService.ShowError("Ошибка при приклеплении данных", "Ошибка отправки данных.");
            }
        }
    }

    public async Task LikeTweet(string tweetId)
    {
        var addTweetUri = $"{Settings.ApiRoot}/v1/Tweets/Like-{tweetId}";
        var response = await _httpClient.PostAsync(addTweetUri, null);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Ошибка при лайке твита.", "Ошибка.");
        }
    }

    private async Task<int> GetCountOfLikes(Guid tweetId)
    {
        var getTweetsLikes = $"{Settings.ApiRoot}/v1/Tweets/likes-{tweetId}";
        var response = await _httpClient.GetAsync(getTweetsLikes);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о твитах.", "Ошибка при получении данных.");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<int>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
        return data;
    }

    private async Task<IEnumerable<string>> GetAttachments(Guid tweetId)
    {
        var getTweetsAttachments = $"{Settings.ApiRoot}/v1/TwitterFiles/get-tweet-files?tweetId={tweetId}";
        var response = await _httpClient.GetAsync(getTweetsAttachments);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о твитах.", "Ошибка при получении данных.");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<IEnumerable<string>>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

        return data;
    }

    private async Task<int> GetCountOfComments(Guid tweetId)
    {
        var getTweetsLikes =
            $"{Settings.ApiRoot}/v1/Comments/get-comment-by-tweet?tweetId={tweetId}&offset=0&limit={int.MaxValue}";
        var response = await _httpClient.GetAsync(getTweetsLikes);

        if (!response.IsSuccessStatusCode)
        {
            _userDialogService.ShowError("Не удалось получить данные о твитах.", "Ошибка при получении данных.");
        }

        var content = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<IEnumerable<CommentResponse>>(content,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true})!.ToList();
        return data.Count;
    }
}