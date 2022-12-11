using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Exceptions;
using Shared.Extensions;
using Twitter.Entities.Base;
using Twitter.Entities.Comments;
using Twitter.Entities.Messenger;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;
using Twitter.FileService.Models;
using Twitter.Repository;

namespace Twitter.FileService;

public class FileService : IFileService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Tweet> _tweetsRepository;
    private readonly IRepository<FileTweet> _fileTweetRepository;
    private readonly IRepository<Comment> _commentsRepository;
    private readonly IRepository<FileComment> _fileCommentRepository;
    private readonly IRepository<TwitterUser> _userRepository;
    private readonly IRepository<Message> _messageRepository;
    private readonly IRepository<MessageFile> _messageFileRepository;
    private readonly IRepository<TwitterFile> _filesRepository;
    private readonly Guid _currentUserId;

    public FileService(IRepository<TwitterFile> filesRepository, IMapper mapper, IRepository<Tweet> tweetsRepository,
        IRepository<FileTweet> fileTweetRepository, IHttpContextAccessor accessor,
        IRepository<Comment> commentsRepository, IRepository<FileComment> fileCommentRepository,
        IRepository<TwitterUser> userRepository, IRepository<Message> messageRepository, IRepository<MessageFile> messageFileRepository)
    {
        _filesRepository = filesRepository;
        _mapper = mapper;
        _tweetsRepository = tweetsRepository;
        _fileTweetRepository = fileTweetRepository;
        _commentsRepository = commentsRepository;
        _fileCommentRepository = fileCommentRepository;
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _messageFileRepository = messageFileRepository;

        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value != null ? Guid.Parse(value) : Guid.Empty;
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
        ProcessException.ThrowIf(() => _currentUserId != _tweetsRepository.GetById(tweetId).CreatorId,
            "Only the creator can change a tweet.");
        ProcessException.ThrowIf(() => files.Any(x => !x.IsImage()), "You can only attach pictures to a tweet!");
        ProcessException.ThrowIf(() => files.Count() > 10, "You can attach maximum 10 files!");

        var createdFiles = new List<TwitterFile>();

        foreach (var file in files)
            if (file.Length > 0)
            {
                var fileModelRequest = new TwitterFileModelRequest();
                var name = Path.GetRandomFileName();

                fileModelRequest.Name = name;
                fileModelRequest.TypeOfFile = TypeOfFile.Tweet;


                var mapFile = _mapper.Map<TwitterFile>(fileModelRequest);
                var createdFile = _filesRepository.Save(mapFile);
                createdFiles.Add(createdFile);
                _fileTweetRepository.Save(new FileTweet() {FileId = createdFile.Id, TweetId = tweetId});

                var filePath = Path.Combine(createdFile.TypeOfFile.GetPath() + '\\', name);
                await using var stream = File.Create(filePath);
                await file.CopyToAsync(stream);
            }

        return createdFiles.Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    public async Task<IEnumerable<TwitterFileModel>> AddFileToComment(IEnumerable<IFormFile> files, Guid commentId)
    {
        ProcessException.ThrowIf(() => _currentUserId != _commentsRepository.GetById(commentId).CreatorId,
            "Only the creator can change a comment.");
        ProcessException.ThrowIf(() => files.Any(x => !x.IsImage()), "You can only attach pictures to a comment!");
        ProcessException.ThrowIf(() => files.Count() > 10, "You can attach maximum 10 files!");

        var createdFiles = new List<TwitterFile>();

        foreach (var file in files)
            if (file.Length > 0)
            {
                var fileModelRequest = new TwitterFileModelRequest();
                var name = Path.GetRandomFileName();

                fileModelRequest.Name = name;
                fileModelRequest.TypeOfFile = TypeOfFile.Comment;

                var createdFile = _filesRepository.Save(_mapper.Map<TwitterFile>(fileModelRequest));
                createdFiles.Add(createdFile);
                _fileCommentRepository.Save(new FileComment() {FileId = createdFile.Id, CommentId = commentId});

                var filePath = Path.Combine(createdFile.TypeOfFile.GetPath() + '\\', name);
                await using var stream = File.Create(filePath);
                await file.CopyToAsync(stream);
            }

        return createdFiles.Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    public async Task<TwitterFileModel> AddAvatar(IFormFile file)
    {
        var user = _userRepository.GetById(_currentUserId);
        if (file.Length > 0)
        {
            var fileModelRequest = new TwitterFileModelRequest();
            var name = Path.GetRandomFileName();

            fileModelRequest.Name = name;
            fileModelRequest.TypeOfFile = TypeOfFile.Avatar;

            var createdFile = _filesRepository.Save(_mapper.Map<TwitterFile>(fileModelRequest));
            user.AvatarId = createdFile.Id;
            _userRepository.Save(user);
            var filePath = Path.Combine(createdFile.TypeOfFile.GetPath() + '\\', name);
            await using var stream = File.Create(filePath);
            await file.CopyToAsync(stream);
            return _mapper.Map<TwitterFileModel>(createdFile);
        }
        
        throw new ProcessException(ErrorMessage.NotFoundError);
    }

    public async Task<IEnumerable<TwitterFileModel>> AddFileToMessage(IEnumerable<IFormFile> files, Guid messageId)
    {
        ProcessException.ThrowIf(() => _currentUserId != _messageRepository.GetById(messageId).SenderId,
            "Only the creator can change a message.");
        ProcessException.ThrowIf(() => files.Any(x => !x.IsImage()), "You can only attach pictures to a message!");
        ProcessException.ThrowIf(() => files.Count() > 10, "You can attach maximum 10 files!");

        var createdFiles = new List<TwitterFile>();

        foreach (var file in files)
            if (file.Length > 0)
            {
                var fileModelRequest = new TwitterFileModelRequest();
                var name = Path.GetRandomFileName();

                fileModelRequest.Name = name;
                fileModelRequest.TypeOfFile = TypeOfFile.Message;

                var createdFile = _filesRepository.Save(_mapper.Map<TwitterFile>(fileModelRequest));
                createdFiles.Add(createdFile);
                _messageFileRepository.Save(new MessageFile() {FileId = createdFile.Id, MessageId = messageId});

                var filePath = Path.Combine(createdFile.TypeOfFile.GetPath() + '\\', name);
                await using var stream = File.Create(filePath);
                await file.CopyToAsync(stream);
            }

        return createdFiles.Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    public Task<IEnumerable<string>> GetTweetFiles(Guid tweetId)
    {
        var files = _fileTweetRepository.GetAll(x => x.TweetId == tweetId);

        var result = new List<string>();
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

        var result = new List<string>();
        foreach (var file in files)
        {
            var path = TypeOfFile.Comment.GetPath() + '\\' + file.File.Name;
            var bytes = File.ReadAllBytes(path);
            result.Add(Convert.ToBase64String(bytes));
        }

        return Task.FromResult<IEnumerable<string>>(result);
    }

    public Task<IEnumerable<string>> GetMessageFiles(Guid messageId)
    {
        var files = _messageFileRepository.GetAll(x => x.MessageId == messageId);

        var result = new List<string>();
        foreach (var file in files)
        {
            var path = TypeOfFile.Message.GetPath() + '\\' + file.File.Name;
            var bytes = File.ReadAllBytes(path);
            result.Add(Convert.ToBase64String(bytes));
        }

        return Task.FromResult<IEnumerable<string>>(result);
    }

    public string GetAvatar(Guid userId)
    {
        try
        {
            var file = _userRepository.GetById(userId).Avatar;

            var path = TypeOfFile.Avatar.GetPath() + '\\' + file.Name;
            var bytes = File.ReadAllBytes(path);

            return Convert.ToBase64String(bytes);
        }
        catch (Exception exception)
        {
            return "";
        }
    }

    public async Task<IEnumerable<TwitterFileModel>> AddFileToTweet(IEnumerable<string> files, Guid tweetId)
    {
        ProcessException.ThrowIf(() => _currentUserId != _tweetsRepository.GetById(tweetId).CreatorId,
            "Only the creator can change a tweet.");
        ProcessException.ThrowIf(() => files.Count() > 10, "You can attach maximum 10 files!");

        var createdFiles = new List<TwitterFile>();

        foreach (var file in files)
            if (file.Length > 0)
            {
                var fileModelRequest = new TwitterFileModelRequest();
                var name = Path.GetRandomFileName();

                fileModelRequest.Name = name;
                fileModelRequest.TypeOfFile = TypeOfFile.Tweet;


                var mapFile = _mapper.Map<TwitterFile>(fileModelRequest);
                var createdFile = _filesRepository.Save(mapFile);
                createdFiles.Add(createdFile);
                _fileTweetRepository.Save(new FileTweet() {FileId = createdFile.Id, TweetId = tweetId});

                var filePath = Path.Combine(createdFile.TypeOfFile.GetPath() + '\\', name);
                await using var stream = File.Create(filePath); 
                var bytes = Convert.FromBase64String(file);
                stream.Write(bytes);
            }

        return createdFiles.Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    public async Task<IEnumerable<TwitterFileModel>> AddFileToComment(IEnumerable<string> files, Guid commentId)
    {
        ProcessException.ThrowIf(() => _currentUserId != _commentsRepository.GetById(commentId).CreatorId,
            "Only the creator can change a comment.");
        ProcessException.ThrowIf(() => files.Count() > 10, "You can attach maximum 10 files!");

        var createdFiles = new List<TwitterFile>();

        foreach (var file in files)
            if (file.Length > 0)
            {
                var fileModelRequest = new TwitterFileModelRequest();
                var name = Path.GetRandomFileName();

                fileModelRequest.Name = name;
                fileModelRequest.TypeOfFile = TypeOfFile.Comment;

                var createdFile = _filesRepository.Save(_mapper.Map<TwitterFile>(fileModelRequest));
                createdFiles.Add(createdFile);
                _fileCommentRepository.Save(new FileComment() {FileId = createdFile.Id, CommentId = commentId});

                var filePath = Path.Combine(createdFile.TypeOfFile.GetPath() + '\\', name);
                await using var stream = File.Create(filePath); 
                var bytes = Convert.FromBase64String(file);
                stream.Write(bytes);
            }

        return createdFiles.Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    public async Task<TwitterFileModel> AddAvatar(string file)
    {
        var user = _userRepository.GetById(_currentUserId);
        if (file.Length > 0)
        {
            var fileModelRequest = new TwitterFileModelRequest();
            var name = Path.GetRandomFileName();

            fileModelRequest.Name = name;
            fileModelRequest.TypeOfFile = TypeOfFile.Avatar;

            var createdFile = _filesRepository.Save(_mapper.Map<TwitterFile>(fileModelRequest));
            user.AvatarId = createdFile.Id;
            _userRepository.Save(user);
            var filePath = Path.Combine(createdFile.TypeOfFile.GetPath() + '\\', name);
            await using var stream = File.Create(filePath); 
            var bytes = Convert.FromBase64String(file);
            stream.Write(bytes);
            return _mapper.Map<TwitterFileModel>(createdFile);
        }
        
        throw new ProcessException(ErrorMessage.NotFoundError);
    }

    public Task<IEnumerable<TwitterFileModel>> AddFileToMessage(IEnumerable<string> files, Guid messageId)
    {
        throw new NotImplementedException();
    }
}