namespace Broker;

public class CompletedRequest
{
    public string EmailAddress { get; set; }
    public List<Quotation> Quotes { get; set; }
}