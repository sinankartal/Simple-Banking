using Persistence.IRepositories;
using Persistence.Models;

namespace Persistence.Data;

public class AccountHolderRepository: Repository<AccountHolder>, IAccountHolderRepository
{
    private readonly ApplicationDbContext _context;

    public AccountHolderRepository(ApplicationDbContext context):base(context)
    {
        _context = context;
    }

    public AccountHolder FindByBSNAsync(string bsn)
    {
       return _context.AccountHolders.FirstOrDefault(a=>a.BSN.Equals(bsn));
    }
}