using System.ComponentModel;

namespace Common.Enums;

public enum TransactionType
{
    [Description("ACCOUNT_CREATE")]
    ACCOUNT_CREATE,
    
    [Description("MONEY_TRANSFER")]
    MONEY_TRANSFER,
    
    [Description("TOPUP")]
    TOPUP,
    
    [Description("FEE")]
    FEE
}