using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Twitter.Entities.Users;
using Twitter.Repository;
using Twitter.RoleService.Models;

namespace Twitter.RoleService;

public class RoleService : IRoleService
{
    private readonly IRepository<TwitterRole> _repository;
    private readonly IMapper _mapper;

    public RoleService(IRepository<TwitterRole> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


    public async Task<IEnumerable<TwitterRoleModel>> GetRoles()
    {
        var roles = _repository.GetAll();
        var result = (await roles.ToListAsync()).Select(x => _mapper.Map<TwitterRoleModel>(x));
        return result;
    }

    public Task<TwitterRoleModel> GetRoleById(Guid id)
    {
        var role = _repository.GetById(id);
        return Task.FromResult(_mapper.Map<TwitterRoleModel>(role));
    }

    public Task DeleteRole(Guid id)
    {
        _repository.Delete(_repository.GetById(id));
        return Task.CompletedTask;
    }

    public Task<TwitterRoleModel> AddRole(TwitterRoleModelRequest requestModel)
    {
        var model = _mapper.Map<TwitterRole>(requestModel);
        return Task.FromResult(_mapper.Map<TwitterRoleModel>(_repository.Save(model)));
    }

    public Task<TwitterRoleModel> UpdateRole(Guid id, TwitterRoleModelRequest requestModel)
    {
        var model = _repository.GetById(id);
        var file = _mapper.Map(requestModel, model);
        return Task.FromResult(_mapper.Map<TwitterRoleModel>(_repository.Save(file)));
    }
}