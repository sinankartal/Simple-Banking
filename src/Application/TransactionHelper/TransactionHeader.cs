using Common.Enums;
using Persistence.Models;

namespace Application.TransactionHelper;

public class TransactionHeader
{
    public bool isLimitCheck { get; set; }
    
    public TransactionType Type { get; set; }
    
    public TransactionFee Fee { get; set; }
}