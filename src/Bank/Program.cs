using System.Reflection.Metadata;
using Azure.Messaging.ServiceBus;
using Bank;
using Microsoft.Extensions.Options;
using Azure.Identity;


var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((ctx, configBuilder)=>
{
    configBuilder.AddAzureKeyVault(new Uri(ctx.Configuration["KeyVaultUri"]), new DefaultAzureCredential());
});

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