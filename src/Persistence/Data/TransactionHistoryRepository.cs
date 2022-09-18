using Persistence.IRepositories;
using Persistence.Models;

namespace Persistence.Data;

public class TransactionHistoryRepository: Repository<TransactionHistory>, ITransactionHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionHistoryRepository(ApplicationDbContext context):base(context)
    {
        _context = context;
    }
}