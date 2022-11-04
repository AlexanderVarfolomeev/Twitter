using System.Linq.Expressions;
using Twitter.TweetsService.Models;

namespace Twitter.TweetsService;

public interface ITweetsService
{
    Task<IEnumerable<TweetModel>> GetTweets(int limit = 100);
    Task<IEnumerable<TweetModel>> GetTweetsByUserId(Guid userId);
    Task<TweetModel> GetTweetById(Guid id);
    Task DeleteTweet(Guid id);
    Task<TweetModel> AddTweet(TweetModelRequest requestModel);
    Task<TweetModel> UpdateTweet(Guid id, TweetModelRequest requestModel);
    Task LikeTweet(Guid tweetId);
}