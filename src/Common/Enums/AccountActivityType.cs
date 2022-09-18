using System.ComponentModel;

namespace Common.Enums;

public enum AccountActivityType
{
    [Description("MONEY_TRANSFER")]
    MONEY_TRANSFER,
    
    [Description("TOPUP")]
    TOPUP,
}