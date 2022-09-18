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
    public async Task<ActionResult<AccountCreateResponse>> Create(AccountCreateRequest request)
    {
        AccountCreateResponse response = await _accountService.CreateAsync(request);

        return Ok(response);
    }

    [SwaggerOperation(Summary = "Get account by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDTO>> Get(string id)
    {
        AccountDTO accountDto = await _accountService.GetAsync(id);
        return Ok(accountDto);
    }
    
    [SwaggerOperation(Summary = "Get accounts by BSN")]
    [HttpGet("{bsn:length(8)}")]
    public ActionResult<List<AccountDTO>> GetAccountsByBsn(string bsn)
    {
        List<AccountDTO> accountDtos =  _accountService.GetAccountsByBsn(bsn);
        return Ok(accountDtos);
    }
    
    [SwaggerOperation(Summary = "Account Top up")]
    [HttpPost("/topup")]
    public async Task<ActionResult<AccountTopUpResponse>> TopUp(AccountTopUpRequest request)
    {
        AccountTopUpResponse topUpResponse = await _accountService.TopUp(request);
        return Ok(topUpResponse);
    }
    
    // [SwaggerOperation(Summary = "Get the progress of provided job by jobId")]
    // [Authorize]
    // [Route("number:length(10)")]
    // [HttpGet("{id}")]
    // public async Task<ActionResult<AccountDTO>> GetByAccountNumber(string number)
    // {
    //     AccountDTO accountDto = await _accountService.Get(number);
    //     return Ok(accountDto);
    // }

}