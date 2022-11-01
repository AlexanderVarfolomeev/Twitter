using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Twitter.AccountService;
using Twitter.AccountService.Models;
using Twitter.Api.Controllers.AccountController.Models;

namespace Twitter.Api.Controllers.AccountController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TwitterAccountController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAccountService _accountService;

    public TwitterAccountController(IMapper mapper, IAccountService accountService)
    {
        _mapper = mapper;
        _accountService = accountService;
    }
    
    [HttpGet("")]
    public async Task<IEnumerable<TwitterAccountResponse>> GetAccounts()
    {
        return (await _accountService.GetAccounts()).Select(x => _mapper.Map<TwitterAccountResponse>(x));
    }
    
    [HttpGet("{id}")]
    public async Task<TwitterAccountResponse> GetAccountById([FromRoute] Guid id)
    {
        return _mapper.Map<TwitterAccountResponse>(await _accountService.GetAccountById(id));
    }
    
    [HttpPost("")]
    public async Task<TwitterAccountResponse> RegisterAccount([FromBody] TwitterAccountRequest account)
    {
        var model = _mapper.Map<TwitterAccountModelRequest>(account);
        return _mapper.Map<TwitterAccountResponse>(await _accountService.RegisterAccount(model));
    }
    
    [HttpDelete("{id}")]
    public Task DeleteAccount([FromRoute] Guid id)
    {
        _accountService.DeleteAccount(id);
        return Task.CompletedTask;
    }
    
    [HttpPatch("{id}")]
    public async Task<TwitterAccountResponse> UpdateFile([FromRoute] Guid id, [FromBody] TwitterAccountRequest account)
    {
        var model = _mapper.Map<TwitterAccountModelRequest>(account);
        return _mapper.Map<TwitterAccountResponse>(await _accountService.UpdateAccount(id, model));
    }
}