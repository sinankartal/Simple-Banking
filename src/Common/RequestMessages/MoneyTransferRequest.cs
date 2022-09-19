namespace Common.RequestMessages;

public class MoneyTransferRequest: FinancialBaseRequest
{
    public string IBAN { get; set; }
    public string Name { get; set; }

    public string Surname { get; set; }

}