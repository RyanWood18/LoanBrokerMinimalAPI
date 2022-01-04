namespace Broker.Model;

public class LoanRequestWithCreditScore
{
    public  decimal Amount { get; set; }
    public CreditScore CreditScore { get; set; } = null!;

    public string RequestId { get; set; } = null!;
}