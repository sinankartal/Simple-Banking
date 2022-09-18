namespace Persistence.Models;

public class AccountHolder : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    
    public string PhoneNumber { get; set; }
    public string BSN { get; set; }
    public List<Account> Accounts { get; set; }
}