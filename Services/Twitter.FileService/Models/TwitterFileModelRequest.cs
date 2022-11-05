using AutoMapper;
using Microsoft.AspNetCore.Http;
using Twitter.Entities.Base;

namespace Twitter.FileService.Models;

public class TwitterFileModelRequest
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public IFormFile File { get; set; }
}

public class TwitterFileModelRequestProfile : Profile
{
    public TwitterFileModelRequestProfile()
    {
        CreateMap<TwitterFileModelRequest, TwitterFile>();
    }
}