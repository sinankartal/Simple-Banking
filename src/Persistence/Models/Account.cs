using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Models;

public class Account:BaseEntity
{
    public AccountHolder Holder { get; set; }
    public string HolderId { get; set; }
    public string AccountNumber { get; set; }
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
}