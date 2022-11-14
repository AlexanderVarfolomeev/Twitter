using Twitter.TweetsService.Models;

namespace Twitter.TweetsService;

public interface ITweetsService
{
    Task<IEnumerable<TweetModel>> GetTweets(int offset = 0, int limit = 10);
    Task<IEnumerable<TweetModel>> GetTweetsBySubscribes(int offset = 0, int limit = 10);
    Task<IEnumerable<TweetModel>> GetTweetsForLastDays(int days, int offset = 0, int limit = 10);
    Task<IEnumerable<TweetModel>> GetTweetsByUserId(Guid userId, int offset = 0, int limit = 10);
    Task<TweetModel> GetTweetById(Guid id);
    Task DeleteTweet(Guid id);
    Task<TweetModel> AddTweet(TweetModelRequest requestModel);
    Task<TweetModel> UpdateTweet(Guid id, TweetModelRequest requestModel);
    Task LikeTweet(Guid tweetId);
    Task<int> GetCountLikesOfTweet(Guid id);
}