using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Api.Controllers.TwitterFilesController.Models;
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
        this._fileService = fileService;
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

    [HttpPost("")]
    public async Task<TwitterFileResponse> AddFile([FromBody] TwitterFileRequest file)
    {
        var model = _mapper.Map<TwitterFileModelRequest>(file);
        return _mapper.Map<TwitterFileResponse>( await _fileService.AddFile(model));
    }

    [HttpDelete("{id}")]
    public Task DeleteFile([FromRoute] Guid id)
    {
        _fileService.DeleteFile(id);
        return Task.CompletedTask;
    }

    [HttpPut("{id}")]
    public async Task<TwitterFileResponse> UpdateFile([FromRoute] Guid id, [FromBody] TwitterFileRequest file)
    {
        var model = _mapper.Map<TwitterFileModelRequest>(file);
        return _mapper.Map<TwitterFileResponse>(await _fileService.UpdateFile(id, model));
    }
    
}