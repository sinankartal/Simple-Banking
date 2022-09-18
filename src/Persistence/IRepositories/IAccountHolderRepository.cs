using Persistence.Models;

namespace Persistence.IRepositories;

public interface IAccountHolderRepository : IRepository<AccountHolder>
{
    public AccountHolder FindByBSNAsync(string BSN);
}