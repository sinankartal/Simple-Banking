using Common.Enums;

namespace Persistence.Models;

public class TransactionHistory:BaseEntity
{
    public TransactionType Type { get; set; }
    public string AccountHolderId { get; set; }
    public string ReferenceNumber { get; set; }
    public string Data { get; set; }
}