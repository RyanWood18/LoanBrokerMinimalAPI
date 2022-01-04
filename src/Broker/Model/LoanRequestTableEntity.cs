using Azure;
using Azure.Data.Tables;

namespace Broker.Model;

public class LoanRequestTableEntity : ITableEntity
{
    public string NotificationEmail { get; set; } = null!;
    public string RequestStatus { get; set; } = null!;
    public string PartitionKey { get; set; } = null!;
    public string RowKey { get; set; } = null!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}