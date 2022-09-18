using Common.Enums;
using Persistence.IRepositories;
using Persistence.Models;

namespace Persistence.Data;

public class TransactionFeeRepository: Repository<TransactionFee>, ITransactionFeeRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionFeeRepository(ApplicationDbContext context):base(context)
    {
        _context = context;
    }

    public TransactionFee GetFeeByType(TransactionFeeType type)
    {
        return _context.TransactionFees.FirstOrDefault(t => t.Type.Equals(type));
    }
}