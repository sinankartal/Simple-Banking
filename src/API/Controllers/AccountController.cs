using Application.IServices;
using Common.DTOs;
using Common.RequestMessages;
using Common.ResponseMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [SwaggerOperation(Summary = "Create new account")]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<AccountCreateResponse>> Create(AccountCreateRequest request)
    {
        AccountCreateResponse response = await _accountService.CreateAsync(request);

        return Ok(response);
    }

    [SwaggerOperation(Summary = "Get account by id")]
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<AccountDTO>> Get(string id)
    {
        AccountDTO accountDto = await _accountService.GetAsync(id);
        return Ok(accountDto);
    }
    
    [SwaggerOperation(Summary = "Get accounts by BSN")]
    [HttpGet("{bsn:length(9)}")]
    [Authorize]
    public async Task<ActionResult<List<AccountDTO>>> GetAccountsByBsn(string bsn)
    {
        List<AccountDTO> accountDtos =  await _accountService.GetAccountsByBsnAsync(bsn);
        return Ok(accountDtos);
    }
}