using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using DynamoAttribute = Amazon.CDK.AWS.DynamoDB.Attribute;
using Constructs;
using Infra.Common;
using Infra.Config;

namespace Infra.Constructs;

public sealed class StorageConstruct : Construct
{
    public Table EventsTable { get; }

    public StorageConstruct(Construct scope,string id, PlatformConfig config): base(scope, id)
    {
        EventsTable = new Table(this, "EventsTable", new TableProps
        {
            TableName = ResourceNaming.EventsTable(config),

            PartitionKey = new DynamoAttribute
            {
                Name = "EventId",
                Type = AttributeType.STRING
            },

            BillingMode = BillingMode.PAY_PER_REQUEST,

            RemovalPolicy = RemovalPolicy.DESTROY
        });
    }
}