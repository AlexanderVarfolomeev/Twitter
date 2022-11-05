using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Api.Controllers.TwitterFilesController.Models;
using Twitter.Entities.Tweets;
using Twitter.FileService;
using Twitter.FileService.Models;


namespace Twitter.Api.Controllers.TwitterFilesController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class TwitterFilesController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
 
    public TwitterFilesController(IFileService fileService, IMapper mapper)
    {
        _fileService = fileService;
        _mapper = mapper;
        
    }

    [HttpGet("")]
    public async Task<IEnumerable<TwitterFileResponse>> GetFiles()
    {
        return (await _fileService.GetFiles()).Select(x => _mapper.Map<TwitterFileResponse>(x));
    }

    [HttpGet("{id}")]
    public async Task<TwitterFileResponse> GetFileById([FromRoute] Guid id)
    {
        return _mapper.Map<TwitterFileResponse>(await _fileService.GetFileById(id));
    }

    [HttpPost("add-files-to-tweet-{tweetId}")]
    public async Task<IEnumerable<TwitterFileModel>> AddFilesToTweet(IEnumerable<IFormFile> files, [FromRoute] Guid tweetId)
    {
        return (await _fileService.AddFileToTweet(files, tweetId)).Select(x => _mapper.Map<TwitterFileModel>(x));
    }
    
    [HttpPost("add-files-to-comment-{commentId}")]
    public async Task<IEnumerable<TwitterFileModel>> AddFilesToComment(IEnumerable<IFormFile> files, [FromRoute] Guid commentId)
    {
        return (await _fileService.AddFileToComment(files, commentId)).Select(x => _mapper.Map<TwitterFileModel>(x));
    }

    [HttpDelete("{id}")]
    public Task DeleteFile([FromRoute] Guid id)
    {
        _fileService.DeleteFile(id);
        return Task.CompletedTask;
    }

    [HttpGet("file")]
    public FileResult GetStream()
    {
        FileStream fs = new FileStream("asd", FileMode.Open);
        string file_type = "application/pdf";
        string file_name = "PDFIcon.pdf";
        return File(fs, file_type, file_name);
    }
    
    [HttpPut("{id}")]
    public async Task<TwitterFileResponse> UpdateFile([FromRoute] Guid id, [FromBody] TwitterFileRequest file)
    {
        var model = _mapper.Map<TwitterFileModelRequest>(file);
        return _mapper.Map<TwitterFileResponse>(await _fileService.UpdateFile(id, model));
    }
}