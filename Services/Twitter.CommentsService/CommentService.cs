using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Enum;
using Shared.Exceptions;
using Twitter.CommentsService.Models;
using Twitter.Entities.Comments;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;
using Twitter.Repository;

namespace Twitter.CommentsService;

public class CommentService : ICommentsService
{
    private readonly IRepository<Comment> _commentsRepository;
    private readonly IRepository<TwitterUser> _accountsRepository;
    private readonly IRepository<Tweet> _tweetRepository;
    private readonly IMapper _mapper;
    
    private readonly Guid _currentUserId;
    public CommentService(IHttpContextAccessor accessor, IRepository<Comment> commentsRepository,
        IRepository<TwitterUser> accountsRepository, IRepository<Tweet> tweetRepository, IMapper mapper)
    {
        _commentsRepository = commentsRepository;
        _accountsRepository = accountsRepository;
        _tweetRepository = tweetRepository;
        _mapper = mapper;
        
        _currentUserId =  Guid.Parse(accessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
    
    public Task<CommentModel> AddComment(CommentModelRequest modelRequest, Guid tweetId)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");
        
        var model = _mapper.Map<Comment>(modelRequest);
        model.CreatorId = _currentUserId;
        model.TweetId = tweetId;

        return Task.FromResult(_mapper.Map<CommentModel>(_commentsRepository.Save(model)));
    }

    public Task<IEnumerable<CommentModel>> GetCommentsByTweet(Guid tweetId)
    {
        ProcessException.ThrowIf(() =>  IsBanned(_currentUserId), "You are banned!");
        
        var comments = _tweetRepository.GetById(tweetId).Comments.Select(x => _mapper.Map<CommentModel>(x));
        return Task.FromResult(comments);
    }

    public Task DeleteComment(Guid id)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");
        
        var comment = _commentsRepository.GetById(id);
        if (comment.Creator.Id != _currentUserId)
        {
            var isAdmin = _accountsRepository.GetById(_currentUserId).TwitterRoles
                .Any(x => x.Role.Permissions is TwitterPermissions.Admin or TwitterPermissions.FullAccessAdmin);
            
            ProcessException.ThrowIf(() => !isAdmin, "Either the user who created it or the admin can delete the comment");
        }

        _commentsRepository.Delete(_commentsRepository.GetById(id));
        return Task.CompletedTask;
    }

    public Task<CommentModel> UpdateComment(Guid id, CommentModelRequest modelRequest)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");
        
        var model = _commentsRepository.GetById(id);
        var comment = _mapper.Map(modelRequest, model);

        ProcessException.ThrowIf(() => comment.CreatorId != _currentUserId,
            "Only the person who created it can change a comment!");

        return Task.FromResult(_mapper.Map<CommentModel>(_commentsRepository.Save(comment)));
    }

    public async Task<IEnumerable<CommentModel>> GetCommentsByUser(Guid userId)
    {
        ProcessException.ThrowIf(() =>_currentUserId != Guid.Empty && IsBanned(_currentUserId), "You are banned!");
        
        _accountsRepository.GetById(userId);
        var list = (await _commentsRepository.GetAll(x => x.Creator.Id == userId).ToListAsync())
            .Select(x => _mapper.Map<CommentModel>(x));
        return list;
    }
    

    private bool IsBanned(Guid userId)
    {
        return _accountsRepository.GetById(userId).IsBanned;
    }
}