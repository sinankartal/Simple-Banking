using Common.Enums;

namespace Persistence.Models;

public class TransactionLimit : BaseEntity
{
    public decimal MaxAmount { get; set; }
    public decimal MinAmount { get; set; }

    public TransactionLimitType Type { get; set; }
}