namespace Common.DTOs;

public class AccountDTO
{
    public string Id { get; set; }
    public String AccountNumber { get; set; }
    public String IBAN { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreateDate { get; set; }
    public AccountHolderDTO AccountHolder { get; set; }
}