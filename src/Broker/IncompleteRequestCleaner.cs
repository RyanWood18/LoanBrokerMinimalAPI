using Azure;
using Azure.Data.Tables;
using Broker.Model;

namespace Broker;

public class IncompleteRequestCleaner : BackgroundService
{
    private readonly QuotationNotifier _quotationNotifier;
    private readonly TableClient _quotationClient;
    private readonly TableClient _requestClient;

    public IncompleteRequestCleaner(TableServiceClient tableServiceClient, QuotationNotifier quotationNotifier)
    {
        _quotationNotifier = quotationNotifier;
        _requestClient = tableServiceClient.GetTableClient("LoanRequests");
        _quotationClient = tableServiceClient.GetTableClient("Quotations");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var loanRequest = _requestClient.Query<LoanRequestTableEntity>(x =>
                x.RequestStatus != "Completed" && x.Timestamp < DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(1)));

            foreach (var request in loanRequest)
            {
                Pageable<QuotationEntity> quotations =
                    _quotationClient.Query<QuotationEntity>(x => x.PartitionKey == request.RowKey);
                
                var completedQuotation = new CompletedRequest
                {
                    EmailAddress = request.NotificationEmail,
                    Quotes = quotations.Select(x => new Quotation {BankId = x.RowKey, Rate = x.Rate}).ToList()
                };
                
                await _quotationNotifier.Notify(completedQuotation);
                
                request.RequestStatus = "Completed";
                await _requestClient.UpdateEntityAsync(request, ETag.All);
            }

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}