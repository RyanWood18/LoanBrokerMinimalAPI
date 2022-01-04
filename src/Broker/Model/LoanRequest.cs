namespace Broker.Model;

public class LoanRequest
{
    public  decimal Amount { get; set; }
    public string SSN { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
}