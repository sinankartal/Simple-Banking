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

    public Task<Account?> GetHolderAccountAsync(string accountHolderId, string accountNumber)
    {
        return _context.Accounts.Where(a => a.AccountNumber.Equals(accountNumber) && a.HolderId.Equals(accountHolderId))
            .FirstOrDefaultAsync();
    }
    
    public override async Task<Account> FindAsync(string id)
    {
        return await _context.Accounts.Include(a => a.Holder).FirstOrDefaultAsync(s=>s.Id.Equals(id));
    }

    public async Task<List<Account>> GetAccountsByBsnAsync(string bsn)
    {
        return await _context.Accounts.Where(res => res.Holder.BSN.Equals(bsn)).Include(s=>s.Holder).ToListAsync();
    }
    
    public async Task<Account> FindByIBANAsync(string iban)
    {
        return await _context.Accounts.Where(res => res.IBAN.Equals(iban)).Include(s=>s.Holder).FirstOrDefaultAsync();
    }
}