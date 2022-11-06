using AutoMapper;
using Shared;
using Twitter.FileService.Models;

namespace Twitter.Api.Controllers.TwitterFilesController.Models;

public class TwitterFileRequest
{
    public string Name { get; set; } = string.Empty;
    public TypeOfFile Type { get; set; }
    public IFormFile File { get; set; }
}

public class TwitterFileRequestProfile : Profile
{
    public TwitterFileRequestProfile()
    {
        CreateMap<TwitterFileRequest, TwitterFileModelRequest>();
    }
}