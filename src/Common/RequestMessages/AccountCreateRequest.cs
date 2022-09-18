namespace Common.RequestMessages;

public class AccountCreateRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    
    public string PhoneNumber { get; set; }
    public string BSN { get; set; }
}