using Common.Enums;
using Persistence.Models;

namespace Persistence.IRepositories;

public interface ITransactionFeeRepository:IRepository<TransactionFee>
{
    TransactionFee GetFeeByType(TransactionFeeType type);
}