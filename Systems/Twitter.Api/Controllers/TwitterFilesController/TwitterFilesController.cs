using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Security;
using Twitter.Api.Controllers.TwitterFilesController.Models;
using Twitter.FileService;
using Twitter.FileService.Models;


namespace Twitter.Api.Controllers.TwitterFilesController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TwitterFilesController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public TwitterFilesController(IFileService fileService, IMapper mapper)
    {
        _fileService = fileService;
        _mapper = mapper;
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("")]
    public async Task<IEnumerable<TwitterFileResponse>> GetFiles()
    {
        return (await _fileService.GetFiles()).Select(x => _mapper.Map<TwitterFileResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("{id}")]
    public async Task<TwitterFileResponse> GetFileById([FromRoute] Guid id)
    {
        return _mapper.Map<TwitterFileResponse>(await _fileService.GetFileById(id));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("add-files-to-tweet")]
    public async Task<IEnumerable<TwitterFileModel>> AddFilesToTweet( IEnumerable<IFormFile> files,
        [FromQuery] Guid tweetId)
    {
        return (await _fileService.AddFileToTweet(files, tweetId)).Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("add-base64Files-to-tweet")]
    public async Task<IEnumerable<TwitterFileModel>> AddBase64FilesToTweet([FromBody] IEnumerable<string> files,
        [FromQuery] Guid tweetId)
    {
        return (await _fileService.AddFileToTweet(files, tweetId)).Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("add-files-to-comment")]
    public async Task<IEnumerable<TwitterFileModel>> AddFilesToComment(IEnumerable<IFormFile> files,
        [FromQuery] Guid commentId)
    {
        return (await _fileService.AddFileToComment(files, commentId)).Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("add-base64Files-to-comment")]
    public async Task<IEnumerable<TwitterFileModel>> AddBase64FilesToComment([FromBody] IEnumerable<string> files,
        [FromQuery] Guid commentId)
    {
        return (await _fileService.AddFileToComment(files, commentId)).Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("add-avatar")]
    public async Task<TwitterFileModel> AddAvatar( IFormFile file)
    {
        return _mapper.Map<TwitterFileModel>(await _fileService.AddAvatar(file));
    }
    
    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("add-base64Avatar")]
    public async Task<TwitterFileModel> AddBase64Avatar([FromBody] string file)
    {
        return _mapper.Map<TwitterFileModel>(await _fileService.AddAvatar(file));
    }


    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-tweet-files")]
    public async Task<IEnumerable<string>> GetTweetFiles([FromQuery] Guid tweetId)
    {
        return await _fileService.GetTweetFiles(tweetId);
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-comment-files")]
    public async Task<IEnumerable<string>> GetCommentFiles([FromQuery] Guid commentId)
    {
        return await _fileService.GetCommentFiles(commentId);
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-avatar")]
    public string GetAvatar([FromQuery] Guid userId)
    {
        return _fileService.GetAvatar(userId);
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpDelete("{id}")]
    public Task DeleteFile([FromRoute] Guid id)
    {
        _fileService.DeleteFile(id);
        return Task.CompletedTask;
    }
}