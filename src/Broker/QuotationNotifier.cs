using SendGrid;
using SendGrid.Helpers.Mail;

namespace Broker;

public class QuotationNotifier
{
    private readonly SendGridClient _emailClient;

    public QuotationNotifier(SendGridClient emailClient)
    {
        this._emailClient = emailClient;
    }

    public async Task Notify(CompletedRequest request)
    {
        var message = CreateMessage(request.EmailAddress, request.Quotes);
        await _emailClient.SendEmailAsync(message);
    }
    
    private static SendGridMessage CreateMessage(string toAddress, List<Quotation> quotes)
    {
        var message = new SendGridMessage();
        message.AddTo(toAddress);
        message.SetSubject(quotes.Count > 0 ? "Quotes Received" : "An update on your loan quote request");
        message.SetFrom(Environment.GetEnvironmentVariable("FromEmailAddress"));
        message.AddContent("text/html", EmailBuilder.BuildEmail(quotes));
        return message;
    }
}