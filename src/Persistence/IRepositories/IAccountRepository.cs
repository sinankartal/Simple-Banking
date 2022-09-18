
using Persistence.Models;

namespace Persistence.IRepositories;

public interface IAccountRepository: IRepository<Account>
{
    Account GetHolderAccount(string accountHolderId, string accountNumber);

    List<Account> GetAccountsByBsn(string bsn);
}