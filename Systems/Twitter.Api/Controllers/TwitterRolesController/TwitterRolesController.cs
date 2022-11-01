using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Twitter.Api.Controllers.TwitterRolesController.Models;
using Twitter.Api.Controllers.TwitterRolesController.Models;
using Twitter.RoleService.Models;
using Twitter.RoleService;
using Twitter.RoleService.Models;

namespace Twitter.Api.Controllers.TwitterRolesController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TwitterRolesController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;

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
        return _mapper.Map<TwitterRoleResponse>( await _roleService.AddRole(model));
    }

    [HttpDelete("{id}")]
    public Task DeleteRole([FromRoute] Guid id)
    {
        _roleService.DeleteRole(id);
        return Task.CompletedTask;
    }

    [HttpPatch("{id}")]
    public async Task<TwitterRoleResponse> UpdateRole([FromRoute] Guid id, [FromBody] TwitterRoleRequest role)
    {
        var model = _mapper.Map<TwitterRoleModelRequest>(role);
        return _mapper.Map<TwitterRoleResponse>(await _roleService.UpdateRole(id, model));
    }
}