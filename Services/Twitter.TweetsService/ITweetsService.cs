using Twitter.TweetsService.Models;

namespace Twitter.TweetsService;

public interface ITweetsService
{
    IEnumerable<TweetModel> GetTweets(int offset = 0, int limit = 10);
    IEnumerable<TweetModel> GetTweetsBySubscribes(int offset = 0, int limit = 10);
    IEnumerable<TweetModel> GetTweetsForLastDays(int days, int offset = 0, int limit = 10);
    IEnumerable<TweetModel> GetTweetsByUserId(Guid userId, int offset = 0, int limit = 10);
    TweetModel GetTweetById(Guid id);
    void DeleteTweet(Guid id);
    TweetModel AddTweet(TweetModelRequest requestModel);
    TweetModel UpdateTweet(Guid id, TweetModelRequest requestModel);
    void LikeTweet(Guid tweetId);
    int GetCountLikesOfTweet(Guid id);
}