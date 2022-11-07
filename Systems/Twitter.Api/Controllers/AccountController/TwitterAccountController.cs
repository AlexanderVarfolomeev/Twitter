using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Security;
using Twitter.AccountService;
using Twitter.AccountService.Models;
using Twitter.Api.Controllers.AccountController.Models;

namespace Twitter.Api.Controllers.AccountController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TwitterAccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public TwitterAccountController(IMapper mapper, IAccountService accountService)
    {
        _mapper = mapper;
        _accountService = accountService;
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("")]
    public async Task<IEnumerable<TwitterAccountResponse>> GetAccounts()
    {
        return (await _accountService.GetAccounts()).Select(x => _mapper.Map<TwitterAccountResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
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

    [Authorize(AppScopes.TwitterWrite)]
    [HttpDelete("{id}")]
    public Task DeleteAccount([FromRoute] Guid id)
    {
        _accountService.DeleteAccount(id);
        return Task.CompletedTask;
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPut("{id}")]
    public async Task<TwitterAccountResponse> UpdateAccount([FromRoute] Guid id,
        [FromBody] TwitterAccountRequest account)
    {
        var model = _mapper.Map<TwitterAccountModelRequest>(account);
        return _mapper.Map<TwitterAccountResponse>(await _accountService.UpdateAccount(id, model));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("Subscribe-{id}")]
    public Task<IActionResult> SubscribeTo([FromRoute] Guid id)
    {
        _accountService.Subscribe(id);
        return Task.FromResult<IActionResult>(Ok());
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPut("ban-user-{id}")]
    public Task<IActionResult> BanUser([FromRoute] Guid id)
    {
        _accountService.BanUser(id);
        return Task.FromResult<IActionResult>(Ok());
    }
}