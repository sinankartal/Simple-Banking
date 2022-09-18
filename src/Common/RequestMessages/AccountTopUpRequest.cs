namespace Common.RequestMessages;

public class AccountTopUpRequest
{
    public string AccountHolderId { get; set; }

    public string AccountNumber { get; set; }
    
    public decimal Amount { get; set; }
}