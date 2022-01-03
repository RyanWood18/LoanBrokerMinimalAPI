namespace Bank.Model;

public record QuotationRequest
{
    public QuotationRequest(decimal amount, string requestId, CreditScore creditScore)
    {
        Amount = amount;
        RequestId = requestId;
        CreditScore = creditScore;
    }
    public decimal Amount { get; set; }
    public string RequestId { get; set; }
    public CreditScore CreditScore { get; set; }
}