namespace Persistence.Models;

public class IBANStore
{
    public int Id { get; set; }
    public string IBAN { get; set; }
    
    public string AccountNumber { get; set; }
    public bool IsActive { get; set; }
    
}