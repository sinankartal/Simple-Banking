using Common.DTOs;
using Common.RequestMessages;
using Common.ResponseMessages;

namespace Application.IServices;

public interface IAccountService
{
    Task<AccountCreateResponse> CreateAsync(AccountCreateRequest accountCreateRequest);
    
    Task<AccountDTO> GetAsync(string id);

    List<AccountDTO> GetAccountsByBsn(string bsn);
}