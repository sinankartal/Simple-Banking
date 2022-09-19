using Application.IServices;
using Common.Enums;
using Common.RequestMessages;
using Common.ResponseMessages;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class FinancialOperationController : ControllerBase
{
    private readonly Func<AccountActivityType, IFinancialServices> _serviceResolver;

    public FinancialOperationController(Func<AccountActivityType, IFinancialServices> serviceResolver)
    {
        _serviceResolver = serviceResolver;
    }

    [SwaggerOperation(Summary = "Account Top up")]
    [HttpPost("/topup")]
    public async Task<ActionResult<AccountTopUpResponse>> TopUp(AccountTopUpRequest request)
    {
        var service = _serviceResolver(AccountActivityType.TOPUP);
        AccountTopUpResponse topUpResponse = await service.ExecuteAsync(request) as AccountTopUpResponse;
        return Ok(topUpResponse);
    }
    
    [SwaggerOperation(Summary = "Money Transfer")]
    [HttpPost("/money-transfer")]
    public async Task<ActionResult<AccountTopUpResponse>> TopUp(MoneyTransferRequest request)
    {
        var service = _serviceResolver(AccountActivityType.MONEY_TRANSFER);
        MoneyTransferResponse moneyTransferResponse = await service.ExecuteAsync(request) as MoneyTransferResponse;
        return Ok(moneyTransferResponse);
    }
}