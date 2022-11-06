using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Exceptions;
using Shared.Extensions;
using Twitter.Entities.Base;
using Twitter.Entities.Comments;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;
using Twitter.FileService.Models;
using Twitter.Repository;
using Twitter.Settings.Source;

namespace Twitter.FileService;

public class FileService : IFileService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Tweet> _tweetsRepository;
    private readonly ISettingSource _settings;
    private readonly IRepository<FileTweet> _fileTweetRepository;
    private readonly IRepository<Comment> _commentsRepository;
    private readonly IRepository<FileComment> _fileCommentRepository;
    private readonly IRepository<TwitterUser> _userRepository;
    private readonly IRepository<TwitterFile> _filesRepository;
    private readonly Guid _currentUserId;
    public FileService(IRepository<TwitterFile> filesRepository, IMapper mapper, IRepository<Tweet> tweetsRepository, 
        ISettingSource settings, IRepository<FileTweet> fileTweetRepository, IHttpContextAccessor accessor,
        IRepository<Comment> commentsRepository, IRepository<FileComment> fileCommentRepository, IRepository<TwitterUser> userRepository)
    {
        _filesRepository = filesRepository;
        _mapper = mapper;
        _tweetsRepository = tweetsRepository;
        _settings = settings;
        _fileTweetRepository = fileTweetRepository;
        _commentsRepository = commentsRepository;
        _fileCommentRepository = fileCommentRepository;
        _userRepository = userRepository;

        _currentUserId =  Guid.Parse(accessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    public async Task<IEnumerable<TwitterFileModel>> GetFiles()
    {
        var files = _filesRepository.GetAll();
        var result = (await files.ToListAsync()).Select(x => _mapper.Map<TwitterFileModel>(x));
        return result;
    }

    public Task<TwitterFileModel> GetFileById(Guid id)
    {
        var file = _filesRepository.GetById(id);
        return Task.FromResult(_mapper.Map<TwitterFileModel>(file));
    }

    public Task DeleteFile(Guid id)
    {
        _filesRepository.Delete(_filesRepository.GetById(id));
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<TwitterFileModel>> AddFileToTweet(IEnumerable<IFormFile> files, Guid tweetId)
    {
        List<TwitterFile> createdFiles = new List<TwitterFile>();
        
        ProcessException.ThrowIf(() => _currentUserId != _tweetsRepository.GetById(tweetId).CreatorId, "Only the creator can change a tweet.");
        ProcessException.ThrowIf(() => files.Any(x => !x.IsImage()), "You can only attach pictures to a tweet!");
        
        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                TwitterFileModelRequest fileModelRequest = new TwitterFileModelRequest();
                var name = Path.GetRandomFileName();
                
                fileModelRequest.Name = name;
                fileModelRequest.Type = TypeOfFile.Tweet;
                
                var createdFile = _filesRepository.Save(_mapper.Map<TwitterFile>(fileModelRequest));
                createdFiles.Add(createdFile);
                _fileTweetRepository.Save(new FileTweet() {FileId = createdFile.Id, TweetId = tweetId});
                
                var filePath = Path.Combine(createdFile.TypeOfFile.GetPath() + '\\', name);
                await using var stream = File.Create(filePath);
                await file.CopyToAsync(stream);
            }
        }

        return createdFiles.Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    public async Task<IEnumerable<TwitterFileModel>> AddFileToComment(IEnumerable<IFormFile> files, Guid commentId)
    {
        List<TwitterFile> createdFiles = new List<TwitterFile>();
        
        ProcessException.ThrowIf(() => _currentUserId != _commentsRepository.GetById(commentId).CreatorId, "Only the creator can change a comment.");
        ProcessException.ThrowIf(() => files.Any(x => !x.IsImage()), "You can only attach pictures to a comment!");
        
        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                TwitterFileModelRequest fileModelRequest = new TwitterFileModelRequest();
                var name = Path.GetRandomFileName();
                
                fileModelRequest.Name = name;
                fileModelRequest.Type = TypeOfFile.Comment;
                
                var createdFile = _filesRepository.Save(_mapper.Map<TwitterFile>(fileModelRequest));
                createdFiles.Add(createdFile);
                _fileCommentRepository.Save(new FileComment() {FileId = createdFile.Id, CommentId = commentId});
                
                var filePath = Path.Combine(createdFile.TypeOfFile.GetPath() + '\\', name);
                await using var stream = File.Create(filePath);
                await file.CopyToAsync(stream);
            }
        }

        return createdFiles.Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    public Task<IEnumerable<string>> GetTweetFiles(Guid tweetId)
    {
        var files = _fileTweetRepository.GetAll(x => x.TweetId == tweetId);

        List<string> result = new List<string>();
        foreach (var file in files)
        {
            var path = TypeOfFile.Tweet.GetPath() + '\\' + file.File.Name;
            var bytes = File.ReadAllBytes(path);
            result.Add(Convert.ToBase64String(bytes));
        }

        return Task.FromResult<IEnumerable<string>>(result);
    }

    public Task<IEnumerable<string>> GetCommentFiles(Guid commentId)
    {
        var files = _fileCommentRepository.GetAll(x => x.CommentId == commentId);

        List<string> result = new List<string>();
        foreach (var file in files)
        {
            var path = TypeOfFile.Comment + '\\' + file.File.Name;
            var bytes = File.ReadAllBytes(path);
            result.Add(Convert.ToBase64String(bytes));
        }

        return Task.FromResult<IEnumerable<string>>(result);
    }

    public Task<string> GetAvatar(Guid userId)
    {
        var file = _userRepository.GetById(userId).Avatar;

        var path = TypeOfFile.Avatar + '\\' + file.Name;
        var bytes = File.ReadAllBytes(path);

        return Task.FromResult<string>(Convert.ToBase64String(bytes));
    }

    public Task<TwitterFileModel> UpdateFile(Guid id, TwitterFileModelRequest requestModel)
    {
        var model = _filesRepository.GetById(id);
        var file = _mapper.Map(requestModel, model);
        return Task.FromResult(_mapper.Map<TwitterFileModel>(_filesRepository.Save(file)));
    }

}