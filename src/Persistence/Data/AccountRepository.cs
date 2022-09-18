using Microsoft.EntityFrameworkCore;
using Persistence.IRepositories;
using Persistence.Models;

namespace Persistence.Data;

public class AccountRepository : Repository<Account>, IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Account GetHolderAccount(string accountHolderId, string accountNumber)
    {
        return _context.Accounts.Where(a => a.AccountNumber.Equals(accountNumber) && a.HolderId.Equals(accountHolderId))
            .FirstOrDefault();
    }
    
    public override async Task<Account> FindAsync(string id)
    {
        return await _context.Accounts.Include(a => a.Holder).FirstOrDefaultAsync();
    }

    public List<Account> GetAccountsByBsn(string bsn)
    {
        return _context.Accounts.Where(res => res.Holder.BSN.Equals(bsn)).Include(s=>s.Holder).ToList();
    }
}