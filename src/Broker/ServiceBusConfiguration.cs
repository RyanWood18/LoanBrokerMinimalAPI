namespace Broker;

public record ServiceBusConfiguration
{
    public ServiceBusConfiguration(string connectionString, string topic, string queue)
    {
        ConnectionString = connectionString;
        Topic = topic;
        Queue = queue;
    }

    public string ConnectionString { get; set; }
    public string Topic { get; set; }
    public string Queue { get; set; }
}