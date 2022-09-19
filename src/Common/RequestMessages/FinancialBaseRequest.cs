namespace Common.RequestMessages;

public class FinancialBaseRequest
{
    public string AccountHolderId { get; set; }
    public string AccountNumber { get; set; }
    public decimal Amount { get; set; }
}