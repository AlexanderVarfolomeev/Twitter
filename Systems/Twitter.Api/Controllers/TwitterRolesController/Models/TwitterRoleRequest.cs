using AutoMapper;
using FluentValidation;
using Shared.Enum;
using Twitter.RoleService.Models;

namespace Twitter.Api.Controllers.TwitterRolesController.Models;

public class TwitterRoleRequest
{
    public string Name { get; set; }
    public TwitterPermissions Permissions { get; set; }
}

public class TwitterRoleRequestProfile : Profile
{
    public TwitterRoleRequestProfile()
    {
        CreateMap<TwitterRoleRequest, TwitterRoleModelRequest>();
    }
}