using Bank;

var builder = Host.CreateDefaultBuilder(args);
builder
    .ConfigureServices((ctx,services) =>
    {

        services.Configure<BankConfiguration>(ctx.Configuration.GetSection("BankSettings"));
        services.Configure<ServiceBusConfiguration>(ctx.Configuration.GetSection("ServiceBusSettings"));
        services.AddScoped<QuotationProvider>();
        services.AddHostedService<QuotationRequestProcessor>();
    });
 IHost host =    builder.Build();

await host.RunAsync();