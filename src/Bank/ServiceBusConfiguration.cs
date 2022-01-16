namespace Bank;

public record ServiceBusConfiguration
{
    public string ConnectionString { get; set; } = null!;
    public string Subscription { get; set; } = null!;
    public string Topic { get; set; } = null!;
    public string Queue { get; set; } = null!;
}