using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Twitter.AccountService.Models;
using Twitter.Entities.Users;
using Twitter.Repository;

namespace Twitter.AccountService;

public class AccountService : IAccountService
{
    private readonly SignInManager<TwitterUser> _signInManager;
    private readonly UserManager<TwitterUser> _userManager;
    private readonly IRepository<TwitterUser> _repository;
    private readonly IMapper _mapper;

    public AccountService(SignInManager<TwitterUser> signInManager, UserManager<TwitterUser> userManager, IRepository<TwitterUser> repository, IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _repository = repository;
        _mapper = mapper;
    }


    public async Task<IEnumerable<TwitterAccountModel>> GetAccounts()
    {
        var accounts = _repository.GetAll();
        var result = (await accounts.ToListAsync()).Select(x => _mapper.Map<TwitterAccountModel>(x));
        return result;
    }

    public Task<TwitterAccountModel> GetAccountById(Guid id)
    {
        var account = _repository.GetById(id);
        return Task.FromResult(_mapper.Map<TwitterAccountModel>(account));
    }

    public Task DeleteAccount(Guid id)
    {
        var account = _repository.GetById(id);
        _repository.Delete(account);
        return Task.CompletedTask;
    }

    public async Task<TwitterAccountModel> RegisterAccount(TwitterAccountModelRequest requestModel)
    {
        var user = await _userManager.FindByEmailAsync(requestModel.Email);
        if (user != null)
            throw new Exception(); //TODO сделать ошибку 

        user = _mapper.Map<TwitterUser>(requestModel);
        user.PhoneNumberConfirmed = false;
        user.EmailConfirmed = false;
        
        user.Init();
        var result = await _userManager.CreateAsync(user, requestModel.Password);
        if (result.Succeeded)
        {
        }

        return _mapper.Map<TwitterAccountModel>(user);

    }

    public Task<TwitterAccountModel> UpdateAccount(Guid id, TwitterAccountModelRequest requestModel)
    {
        var model = _repository.GetById(id);
        var file = _mapper.Map(requestModel, model);
        return Task.FromResult(_mapper.Map<TwitterAccountModel>(_repository.Save(file)));
    }
}

