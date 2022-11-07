using AutoMapper;
using Shared;
using Twitter.FileService.Models;

namespace Twitter.Api.Controllers.TwitterFilesController.Models;
//TODO убрать?
public class TwitterFileRequest
{
    public string Name { get; set; } = string.Empty;
    public TypeOfFile TypeOfFile { get; set; }
    public IFormFile File { get; set; }
}

public class TwitterFileRequestProfile : Profile
{
    public TwitterFileRequestProfile()
    {
        CreateMap<TwitterFileRequest, TwitterFileModelRequest>();
    }
}