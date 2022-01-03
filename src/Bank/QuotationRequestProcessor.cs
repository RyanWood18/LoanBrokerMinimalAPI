using Azure.Messaging.ServiceBus;
using Bank.Model;
using Microsoft.Extensions.Options;

namespace Bank;

public class QuotationRequestProcessor : IHostedService
{
    private readonly ILogger<QuotationRequestProcessor> _logger;
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _serviceBussProcessor;
    private readonly ServiceBusSender _sender;
    private readonly QuotationProvider _quotationProvider;
    private readonly string _bankId;

    public QuotationRequestProcessor(ILogger<QuotationRequestProcessor> logger, IOptions<ServiceBusConfiguration> sbOptions, QuotationProvider quotationProvider, IOptions<BankConfiguration> bankOptions)
    {
        _logger = logger;
        _quotationProvider = quotationProvider;
        var config = sbOptions.Value;
        _client = new ServiceBusClient(config.ConnectionString);
        _serviceBussProcessor = _client.CreateProcessor(config.Topic, config.Subscription);
        _sender = _client.CreateSender(config.Queue);
        _bankId = bankOptions.Value.BankId;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _serviceBussProcessor.ProcessMessageAsync += MessageHandler;
        _serviceBussProcessor.ProcessErrorAsync += ErrorHandler;
        
        await _serviceBussProcessor.StartProcessingAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _serviceBussProcessor.StopProcessingAsync(cancellationToken);
        }
        finally
        {
            await _serviceBussProcessor.DisposeAsync();
            await _client.DisposeAsync();
        }
    }
    
    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var request = args.Message.Body.ToObjectFromJson<QuotationRequest>();
        if (_quotationProvider.IsEligibleForLoan(request))
        {
            var quotation = _quotationProvider.ProvideQuotation(request);
            _logger.LogInformation($"Loan criteria for request {request.RequestId} accepted at {quotation} by bank {_bankId} ");
            await _sender.SendMessageAsync(new ServiceBusMessage(BinaryData.FromObjectAsJson(quotation)));
        }
        else
        {
            _logger.LogInformation($"Bank {_bankId} did not provide a quote for request {request.RequestId}");
        }
        
    }
    
    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "An error occurred while reading from the quotation topic");
        return Task.CompletedTask;
    }
}