using Azure;
using Azure.Data.Tables;

namespace Broker;

public class LoanRequestTableEntity : ITableEntity
{
    public string NotificationEmail { get; set; }
    public string RequestStatus { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}