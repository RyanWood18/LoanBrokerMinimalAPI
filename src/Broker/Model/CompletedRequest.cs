namespace Broker.Model;

public class CompletedRequest
{
    public string EmailAddress { get; set; } = null!;
    public List<Quotation> Quotes { get; set; } = null!;
}