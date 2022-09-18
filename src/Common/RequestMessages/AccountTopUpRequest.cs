namespace Common.RequestMessages;

public class AccountTopUpRequest: AccountActivitiesRequest
{ 
    public decimal Amount { get; set; }
}