using System.ComponentModel;

namespace Common.Enums;

public enum TransactionType
{
    [Description("MONEY_TRANSFER")]
    MONEY_TRANSFER,
    
    [Description("TOPUP")]
    TOPUP,
    
    [Description("FEE")]
    FEE
}