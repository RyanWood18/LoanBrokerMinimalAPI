using Azure.Data.Tables;
using Azure.Messaging.ServiceBus;
using Broker;
using Broker.Model;
using Microsoft.Extensions.Options;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSendGrid((s, o) =>
{
    var config = s.GetService<IConfiguration>();
    o.ApiKey = config.GetValue<string>("SendGridApiKey");
});

builder.Services.Configure<ServiceBusConfiguration>(builder.Configuration.GetSection("ServiceBusSettings"));

builder.Services.AddSingleton(sp =>
{
    var options = sp.GetService<IOptions<ServiceBusConfiguration>>();
    return new ServiceBusClient(options!.Value.ConnectionString);
});

builder.Services.AddSingleton(sp => new TableServiceClient(sp.GetService<IConfiguration>().GetConnectionString("TableService")));
builder.Services.AddHostedService<QuotationAggregator>();
builder.Services.AddScoped<QuotationRequester>();
builder.Services.AddHostedService<IncompleteRequestCleaner>();

var app = builder.Build();

app.MapPost("/RequestQuotations/", async (LoanRequest request, QuotationRequester requester) =>
{
    await requester.RequestQuotes(request);
    return Results.Accepted();
});

app.Run();