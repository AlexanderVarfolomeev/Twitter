using AutoMapper;
using Twitter.FileService.Models;

namespace Twitter.Api.Controllers.TwitterFilesController.Models;

public class TwitterFileRequest
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class TwitterFileRequestProfile : Profile
{
    public TwitterFileRequestProfile()
    {
        CreateMap<TwitterFileRequest, TwitterFileModelRequest>();
    }
}