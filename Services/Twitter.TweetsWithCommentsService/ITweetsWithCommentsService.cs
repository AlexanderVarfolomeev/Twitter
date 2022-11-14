using System.Runtime.InteropServices;
using TweetService.Models;

namespace TweetService;

public interface ITweetsWithCommentsService
{
    public IEnumerable<TweetModel> GetTweets();
    public IEnumerable<TweetModel> GetTweetsByUserId(Guid userId);
    public TweetModel GetTweetById(Guid id);

    public TweetModel AddTweet(TweetModelRequest tweetModelRequest);

}