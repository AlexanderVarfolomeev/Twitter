using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.WPF.Services.AccountService.Models;
using Twitter.WPF.Services.TweetsService.Models;

namespace Twitter.WPF.Services.TweetsService;

public interface ITweetsService
{
    Task<IEnumerable<TweetView>> GetTweets(int offset = 0, int limit = 10);
    Task<IEnumerable<TweetView>> GetTweetsByUserId(string userId, int offset = 0, int limit = 10);
    Task<IEnumerable<TweetView>> GetTweetsBySubscriptions(int offset = 0, int limit = 10);
    
    Task AddTweet(TweetRequest tweet, List<string> attachments);
    Task LikeTweet(string tweetId);
}