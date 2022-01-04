namespace Broker.Model;

public class Quotation
{
    public double Rate { get; set; }
    public string BankId { get; set; } = null!;
    public string RequestId { get; set; } = null!;
}