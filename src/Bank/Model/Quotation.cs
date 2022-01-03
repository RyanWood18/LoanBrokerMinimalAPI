namespace Bank.Model;

public record Quotation
{
    public Quotation(string status, double? rate, string bankId, string requestId)
    {
        Status = status;
        Rate = rate;
        BankId = bankId;
        RequestId = requestId;
    }

    public string Status { get; set; }
    public double? Rate { get; set; }
    public string BankId { get; set; }
    public string RequestId { get; set; }
}