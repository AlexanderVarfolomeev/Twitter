using Twitter.RoleService.Models;

namespace Twitter.RoleService;

public interface IRoleService
{
    Task<IEnumerable<TwitterRoleModel>> GetRoles();
    Task<TwitterRoleModel> GetRoleById(Guid id);
    Task DeleteRole(Guid id);
    Task<TwitterRoleModel> AddRole(TwitterRoleModelRequest requestModel);
    Task<TwitterRoleModel> UpdateRole(Guid id, TwitterRoleModelRequest requestModel);
}