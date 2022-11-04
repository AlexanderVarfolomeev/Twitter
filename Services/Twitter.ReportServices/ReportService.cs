using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Shared.Enum;
using Shared.Exceptions;
using Twitter.Entities.Base;
using Twitter.Entities.Comments;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;
using Twitter.ReportServices.Models;
using Twitter.Repository;

namespace Twitter.ReportServices;

public class ReportService : IReportService
{
    private readonly IRepository<ReportToComment> _reportToCommentRepository;
    private readonly IRepository<ReportToTweet> _reportToTweetRepository;
    private readonly IRepository<ReasonReport> _reasonReportRepository;
    private readonly IRepository<TwitterUser> _accountRepository;
    private readonly IMapper _mapper;
    
    private readonly Guid _currentUserId;

    public ReportService(IRepository<ReportToComment> reportToCommentRepository, IRepository<ReportToTweet> reportToTweetRepository,
        IRepository<ReasonReport> reasonReportRepository, IHttpContextAccessor accessor, IRepository<TwitterUser> accountRepository, IMapper mapper)
    {
        _reportToCommentRepository = reportToCommentRepository;
        _reportToTweetRepository = reportToTweetRepository;
        this._reasonReportRepository = reasonReportRepository;
        _accountRepository = accountRepository;
        _mapper = mapper;
        
        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value != null ? Guid.Parse(value) : Guid.Empty;
    }
    
    public Task<IEnumerable<ReportModel>> GetReportsToTweets()
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "No access rights!");
        var reportsToTweets =  _reportToTweetRepository.GetAll().Select(x => _mapper.Map<ReportModel>(x));

        return Task.FromResult<IEnumerable<ReportModel>>(reportsToTweets);
    }
    
    public Task<IEnumerable<ReportModel>> GetReportsToComments()
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "No access rights!");
        var reportsToComments =  _reportToCommentRepository.GetAll().Select(x => _mapper.Map<ReportModel>(x));

        return Task.FromResult<IEnumerable<ReportModel>>(reportsToComments);
    }

    public Task<IEnumerable<ReportModel>> GetReportsByTweet(Guid tweetId)
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "No access rights!");
        var reportsToTweets =  _reportToTweetRepository.GetAll(x => x.TweetId == tweetId).Select(x => _mapper.Map<ReportModel>(x));

        return Task.FromResult<IEnumerable<ReportModel>>(reportsToTweets);
    }

    public Task<IEnumerable<ReportModel>> GetReportsByComment(Guid commentId)
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "No access rights!");
        var reportsToComments =  _reportToCommentRepository.GetAll(x => x.CommentId == commentId).Select(x => _mapper.Map<ReportModel>(x));

        return Task.FromResult<IEnumerable<ReportModel>>(reportsToComments);
    }

    public Task CloseReportToTweet(Guid reportId)
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "No access rights!");

        var report = _reportToTweetRepository.GetById(reportId);
        report.CloseDate = DateTime.Now;
        _reportToTweetRepository.Save(report);

        return Task.CompletedTask;
    } 
    
    // При закрытии жалобы, админ решает что делать с юзером (забанить, удалить коммент, твит) - на стороне клиента?
    public Task CloseReportToComment(Guid reportId)
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "No access rights!");

        var report = _reportToCommentRepository.GetById(reportId);
        report.CloseDate = DateTime.Now;
        _reportToCommentRepository.Save(report);

        return Task.CompletedTask;
    }

    public Task<ReportModel> AddTweetReport(ReportModelRequest modelRequest, Guid tweetId)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned.");
        
        var model = _mapper.Map<ReportToTweet>(modelRequest);
        model.CreatorId = _currentUserId;
        model.TweetId = tweetId;
        
        return Task.FromResult(_mapper.Map<ReportModel>(_reportToTweetRepository.Save(model)));
    }

    public Task<ReportModel> AddCommentReport(ReportModelRequest modelRequest, Guid commentId)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned.");
        
        var model = _mapper.Map<ReportToComment>(modelRequest);
        model.CreatorId = _currentUserId;
        model.CommentId = commentId;
        
        return Task.FromResult(_mapper.Map<ReportModel>(_reportToCommentRepository.Save(model)));
    }
    
    private bool IsAdmin(Guid userId)
    {
        return _accountRepository.GetById(userId).TwitterRoles.Any(x =>
            x.Role.Permissions is TwitterPermissions.Admin or TwitterPermissions.FullAccessAdmin);
    }
    
    private bool IsBanned(Guid userId)
    {
        return _accountRepository.GetById(userId).IsBanned;
    }
}