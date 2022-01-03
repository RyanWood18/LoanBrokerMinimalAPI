namespace Broker;

public class LoanRequestWithCreditScore
{
    public  decimal Amount { get; set; }
    public CreditScore CreditScore { get; set; }
        
    public string RequestId { get; set; }
}