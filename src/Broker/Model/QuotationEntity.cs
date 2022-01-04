using Azure;
using Azure.Data.Tables;

namespace Broker.Model;

public class QuotationEntity : ITableEntity
{
    public QuotationEntity(){}
    public QuotationEntity(double rate, string partitionKey, string rowKey) 
    {
        Rate = rate;
        PartitionKey = partitionKey;
        RowKey = rowKey;
    }
    
    public double Rate { get; }
    public string PartitionKey { get; set; } = null!;
    public string RowKey { get; set; } = null!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}