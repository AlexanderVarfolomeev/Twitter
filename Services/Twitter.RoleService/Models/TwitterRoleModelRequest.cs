using AutoMapper;
using Shared.Enum;
using Twitter.Entities.Users;

namespace Twitter.RoleService.Models;

public class TwitterRoleModelRequest
{
    public string Name { get; set; }
    public TwitterPermissions Permissions { get; set; }
}

public class TwitterRoleModelRequestProfile : Profile
{
    public TwitterRoleModelRequestProfile()
    {
        CreateMap<TwitterRoleModelRequest, TwitterRole>();
    }
}