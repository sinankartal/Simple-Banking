using Common.Enums;
using Persistence.Models;

namespace Persistence.IRepositories;

public interface ITransactionLimitRepository
{
    Task<TransactionLimit> GetByTypeAsync(TransactionLimitType type);
}