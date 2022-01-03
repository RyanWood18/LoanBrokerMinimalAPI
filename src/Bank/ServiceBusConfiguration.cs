namespace Bank;

public record ServiceBusConfiguration
{
    public ServiceBusConfiguration(string connectionString, string subscription, string topic, string queue)
    {
        ConnectionString = connectionString;
        Subscription = subscription;
        Topic = topic;
        Queue = queue;
    }

    public string ConnectionString { get; set; }
    public string Subscription { get; set; }
    public string Topic { get; set; }
    public string Queue { get; set; }
}