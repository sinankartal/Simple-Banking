using Application.IServices;
using Common.DTOs;
using Common.Enums;
using Common.RequestMessages;
using Common.ResponseMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountActivitiesController : ControllerBase
{
    private readonly Func<AccountActivityType, IAccountActivitiesService> _serviceResolver;

    public AccountActivitiesController(Func<AccountActivityType, IAccountActivitiesService> serviceResolver)
    {
        _serviceResolver = serviceResolver;
    }

    [SwaggerOperation(Summary = "Account Top up")]
    [HttpPost("/topup")]
    public async Task<ActionResult<AccountTopUpResponse>> TopUp(AccountTopUpRequest request)
    {
        var service = _serviceResolver(AccountActivityType.TOPUP);
        AccountActivitiesResponse topUpResponse = await service.Execute(request);
        return Ok(topUpResponse);
    }
}