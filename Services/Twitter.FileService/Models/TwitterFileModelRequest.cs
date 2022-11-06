using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared;
using Twitter.Entities.Base;

namespace Twitter.FileService.Models;

public class TwitterFileModelRequest
{
    public string Name { get; set; } = string.Empty;
    public TypeOfFile Type { get; set; }
    public IFormFile File { get; set; }
}

public class TwitterFileModelRequestProfile : Profile
{
    public TwitterFileModelRequestProfile()
    {
        CreateMap<TwitterFileModelRequest, TwitterFile>();
    }
}