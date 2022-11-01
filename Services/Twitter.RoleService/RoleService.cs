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
    private readonly IRepository<TwitterRoleTwitterUser> _rolesUserRepository;

    public RoleService(IRepository<TwitterRole> repository, IMapper mapper, IRepository<TwitterRoleTwitterUser> rolesUserRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _rolesUserRepository = rolesUserRepository;
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

    //TODO смотреть кто вызывает метод (роли может выдавать только админ с высшимы правами)
    public Task GiveRole(Guid roleId, Guid userId)
    {
        _rolesUserRepository.Save(new TwitterRoleTwitterUser() {RoleId = roleId, UserId = userId});    
        return Task.CompletedTask;
    }
}