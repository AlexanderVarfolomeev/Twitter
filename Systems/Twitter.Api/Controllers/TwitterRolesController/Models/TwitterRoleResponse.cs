using AutoMapper;
using Shared.Enum;
using Twitter.Api.Controllers.TwitterFilesController.Models;
using Twitter.RoleService.Models;

namespace Twitter.Api.Controllers.TwitterRolesController.Models;

public class TwitterRoleResponse
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public TwitterPermissions Permissions { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
}

public class TwitterRoleResponseProfile : Profile
{
    public TwitterRoleResponseProfile()
    {
        CreateMap<TwitterRoleModel, TwitterRoleResponse>();
    }
}