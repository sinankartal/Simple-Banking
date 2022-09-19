
using Persistence.Models;

namespace Persistence.IRepositories;

public interface IAccountRepository: IRepository<Account>
{
    Task<Account?> GetHolderAccountAsync(string accountHolderId, string accountNumber);

    Task<List<Account>> GetAccountsByBsnAsync(string bsn);

    Task<Account> FindByIBANAsync(string iban);
}