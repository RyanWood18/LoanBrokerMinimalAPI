using Azure.Messaging.ServiceBus;
using Bank;
using Microsoft.Extensions.Options;

var builder = Host.CreateDefaultBuilder(args);
builder
    .ConfigureServices((ctx,services) =>
    {

        services.Configure<BankConfiguration>(ctx.Configuration.GetSection("BankSettings"));
        services.Configure<ServiceBusConfiguration>(ctx.Configuration.GetSection("ServiceBusSettings"));
        services.AddSingleton(sp =>
        {
            var options = sp.GetService<IOptions<ServiceBusConfiguration>>();
            return new ServiceBusClient(options!.Value.ConnectionString);
        });
        services.AddSingleton<QuotationProvider>();
        services.AddHostedService<QuotationRequestProcessor>();
    });
IHost host =    builder.Build();
await host.RunAsync();