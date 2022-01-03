using Azure;
using Azure.Data.Tables;

namespace Broker;

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
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}