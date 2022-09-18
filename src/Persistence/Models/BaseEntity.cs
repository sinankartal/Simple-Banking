namespace Persistence.Models;

public class BaseEntity
{
    public string Id { get; set; }

    public DateTime CreateDate { get; set; }
    public DateTime ModifyDate { get; set; }
}