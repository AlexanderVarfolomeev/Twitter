using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared;
using Shared.Extensions;
using TweetService.Models;
using Twitter.Entities.Comments;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;
using Twitter.Repository;

namespace TweetService;

public class TweetsWithCommentsService : ITweetsWithCommentsService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Tweet> _tweetRepository;
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<FileComment> _fileCommentRepository;
    private readonly IRepository<FileTweet> _fileTweetRepository;
    private readonly IRepository<TwitterUser> _userRepository;
    
    private readonly Guid _currentUserId;

    public TweetsWithCommentsService(IMapper mapper, IRepository<Tweet> tweetRepository, IRepository<Comment> commentRepository,
        IRepository<FileComment> fileCommentRepository,
        IRepository<FileTweet> fileTweetRepository, IHttpContextAccessor accessor,
        IRepository<TwitterUser> userRepository)
    {
        _mapper = mapper;
        _tweetRepository = tweetRepository;
        _commentRepository = commentRepository;
        _fileCommentRepository = fileCommentRepository;
        _fileTweetRepository = fileTweetRepository;
        _userRepository = userRepository;
        
        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value != null ? Guid.Parse(value) : Guid.Empty;
    }

    public IEnumerable<TweetModel> GetTweets()
    {
        var tweets = _tweetRepository.GetAll().ToList();

        return tweets.Select(MapToTweetModel).ToList();
    }

    public IEnumerable<TweetModel> GetTweetsByUserId(Guid userId)
    {
        var tweets = _tweetRepository.GetAll(x => x.CreatorId == userId).ToList();

        return tweets.Select(MapToTweetModel).ToList();
    }

    public TweetModel GetTweetById(Guid id)
    {
        var tweet = _tweetRepository.GetById(id);
        return MapToTweetModel(tweet);
    }

    public TweetModel AddTweet(TweetModelRequest tweetModelRequest)
    {
        throw new NotImplementedException();
    }

    private TweetModel MapToTweetModel(Tweet tweet)
    {
        TweetModel tweetModel = _mapper.Map<TweetModel>(tweet);
        
        tweetModel.CreatorUserName = _userRepository.GetById(tweet.CreatorId).UserName;
        tweetModel.Comments = GetCommentsByTweet(tweet.Id);

        var fileTweets = _fileTweetRepository.GetAll(x => x.TweetId == tweet.Id);
        tweetModel.ImagesId = fileTweets.Select(x => x.FileId);

        return tweetModel;
    }
    
    private IEnumerable<CommentModel> GetCommentsByTweet(Guid tweetId)
    {
        var comments = _commentRepository.GetAll(x => x.TweetId == tweetId).Select(x => _mapper.Map<CommentModel>(x))
            .ToList();

        foreach (var comment in comments)
        {
            comment.CreatorUserName = _userRepository.GetById(comment.CreatorId).UserName;

            var files = _fileCommentRepository.GetAll(x => x.CommentId == comment.Id);
            comment.ImagesId = files.Select(x => x.FileId);
        }

        return comments;
    }
}