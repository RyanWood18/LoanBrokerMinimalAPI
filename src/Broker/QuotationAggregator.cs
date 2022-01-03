using Azure;
using Azure.Data.Tables;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;

namespace Broker;

public class QuotationAggregator : IHostedService
{
    private readonly ServiceBusProcessor _sbProcessor;
    private readonly TableClient _quotationsClient;
    private readonly TableClient _requestsClient;
    private readonly QuotationNotifier _quotationNotifier;
    private readonly ILogger<QuotationAggregator> _logger;

    public QuotationAggregator(ServiceBusClient sbClient, IOptions<ServiceBusConfiguration> sbOptions, TableServiceClient tableServiceClient, QuotationNotifier quotationNotifier, ILogger<QuotationAggregator> logger)
    {
        _quotationNotifier = quotationNotifier;
        _logger = logger;
        _quotationsClient = tableServiceClient.GetTableClient("Quotations");
        _requestsClient = tableServiceClient.GetTableClient("LoanRequests");
        var sbConfig = sbOptions.Value;
        _sbProcessor = sbClient.CreateProcessor(sbConfig.Queue);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _sbProcessor.ProcessMessageAsync += MessageHandler;
        _sbProcessor.ProcessErrorAsync += ErrorHandler;

        await _sbProcessor.StartProcessingAsync(cancellationToken);
    }

    private Task ErrorHandler(ProcessErrorEventArgs arg)
    {
        _logger.LogError(arg.Exception, "An error occurred while trying to read from the quotation queue");
        return Task.CompletedTask;
    }

    private async Task MessageHandler(ProcessMessageEventArgs arg)
    {
        var quote = arg.Message.Body.ToObjectFromJson<Quotation>();
        var quotationEntity = new QuotationEntity(quote.Rate, quote.RequestId, quote.BankId);
        await _quotationsClient.AddEntityAsync(quotationEntity);
        Pageable<QuotationEntity> quotations =
            _quotationsClient.Query<QuotationEntity>(x => x.PartitionKey == quote.RequestId);

        if (quotations.Count() > 2)
        {
            var loanRequest = _requestsClient.Query<LoanRequestTableEntity>(x => x.RowKey == quote.RequestId).First();
            
            loanRequest.RequestStatus = "Completed";
            await _requestsClient.UpdateEntityAsync(loanRequest, loanRequest.ETag);
            
            var completedQuotation = new CompletedRequest
            {
                EmailAddress = loanRequest.NotificationEmail,
                Quotes = quotations.Select(x => new Quotation {BankId = x.RowKey, Rate = x.Rate}).ToList()
            };
            await _quotationNotifier.Notify(completedQuotation);
        }

    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _sbProcessor.StopProcessingAsync(cancellationToken);
        }
        finally
        {
            await _sbProcessor.DisposeAsync();
        }
    }
}