using Microsoft.EntityFrameworkCore;
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

    public async Task<AccountHolder> FindByBSNAsync(string bsn)
    {
       return await _context.AccountHolders.FirstOrDefaultAsync(a=>a.BSN.Equals(bsn));
    }
}