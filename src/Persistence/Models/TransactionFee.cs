using Common.Enums;

namespace Persistence.Models;

public class TransactionFee: BaseEntity
{
    public TransactionFeeType Type { get; set; }
    
    public decimal Percentage { get; set; }
}