using Persistence.Models;

namespace Persistence.IRepositories;

public interface IAccountHolderRepository : IRepository<AccountHolder>
{
    public Task<AccountHolder> FindByBSNAsync(string BSN);
}