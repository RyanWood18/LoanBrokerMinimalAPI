using System.Text.Json;
using Azure.Data.Tables;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;

namespace Broker;

public class QuotationRequester
{
    private readonly ServiceBusClient _sbClient;
    private readonly TableServiceClient _tableServiceClient;
    private readonly ServiceBusConfiguration _sbConfig;
    private readonly string _creditBureauUrl;
    private readonly HttpClient _httpClient = new HttpClient();
    
    public QuotationRequester(ServiceBusClient sbClient, TableServiceClient tableServiceClient, IOptions<ServiceBusConfiguration> sbOptions, IConfiguration config)
    {
        _sbClient = sbClient;
        _tableServiceClient = tableServiceClient;
        _sbConfig = sbOptions.Value;
        _creditBureauUrl = config.GetValue<string>("CreditBureauUrl");
    }

    public async Task RequestQuotes(LoanRequest loanRequest)
    {
        var requestTable = _tableServiceClient.GetTableClient("LoanRequests");
        var request = new LoanRequestTableEntity
        {
            PartitionKey = "Requests", RowKey = Guid.NewGuid().ToString(), RequestStatus = "Initiated",
            NotificationEmail = loanRequest.EmailAddress
        };
        
        await requestTable.AddEntityAsync(request);

        var creditScore = await GetCreditScore(loanRequest.SSN);
        var requestAndScore = new LoanRequestWithCreditScore
            {Amount = loanRequest.Amount, CreditScore = creditScore, RequestId = request.RowKey};

        await using var sender = _sbClient.CreateSender(_sbConfig.Topic);
        await sender.SendMessageAsync(new ServiceBusMessage(BinaryData.FromObjectAsJson(requestAndScore)));
    }

    private async Task<CreditScore> GetCreditScore(string ssn)
    {
        var requestUri = string.Format(_creditBureauUrl, ssn);
        var request = await _httpClient.GetAsync(requestUri);

        var score = JsonSerializer.Deserialize<CreditScore>(await request.Content.ReadAsStringAsync(), new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
        return score;
    }
}