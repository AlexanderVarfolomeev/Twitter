using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Api.Controllers.TwitterRolesController.Models;
using Twitter.RoleService;
using Twitter.RoleService.Models;

namespace Twitter.Api.Controllers.TwitterRolesController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class TwitterRolesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;

    public TwitterRolesController(IRoleService _roleService, IMapper mapper)
    {
        this._roleService = _roleService;
        _mapper = mapper;
    }

    [HttpGet("")]
    public async Task<IEnumerable<TwitterRoleResponse>> GetRoles()
    {
        return (await _roleService.GetRoles()).Select(x => _mapper.Map<TwitterRoleResponse>(x));
    }

    [HttpGet("{id}")]
    public async Task<TwitterRoleResponse> GetRoleById([FromRoute] Guid id)
    {
        return _mapper.Map<TwitterRoleResponse>(await _roleService.GetRoleById(id));
    }

    [HttpPost("")]
    public async Task<TwitterRoleResponse> AddRole([FromBody] TwitterRoleRequest role)
    {
        var model = _mapper.Map<TwitterRoleModelRequest>(role);
        return _mapper.Map<TwitterRoleResponse>(await _roleService.AddRole(model));
    }

    [HttpPost("give-role-{roleId}:{userId}")]
    public async Task<IActionResult> GiveRole([FromRoute] Guid roleId, [FromRoute] Guid userId)
    {
        _roleService.GiveRole(roleId, userId);
        return Ok();
    }

    [HttpPost("revoke-role-{roleId}:{userId}")]
    public async Task<IActionResult> RevokeRole([FromRoute] Guid roleId, [FromRoute] Guid userId)
    {
        _roleService.RevokeRole(roleId, userId);
        return Ok();
    }

    [HttpDelete("{id}")]
    public Task DeleteRole([FromRoute] Guid id)
    {
        _roleService.DeleteRole(id);
        return Task.CompletedTask;
    }

    [HttpPut("{id}")]
    public async Task<TwitterRoleResponse> UpdateRole([FromRoute] Guid id, [FromBody] TwitterRoleRequest role)
    {
        var model = _mapper.Map<TwitterRoleModelRequest>(role);
        return _mapper.Map<TwitterRoleResponse>(await _roleService.UpdateRole(id, model));
    }
}