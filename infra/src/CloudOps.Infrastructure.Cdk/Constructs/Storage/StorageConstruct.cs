using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using DynamoAttribute = Amazon.CDK.AWS.DynamoDB.Attribute;
using Constructs;
using CloudOps.Infrastructure.Cdk.Common;
using CloudOps.Infrastructure.Cdk.Configuration;

namespace CloudOps.Infrastructure.Cdk.Constructs.Storage;

public sealed class StorageConstruct : Construct
{
    public Table EventsTable { get; }

    public StorageConstruct(Construct scope,string id, PlatformConfiguration config): base(scope, id)
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

        new CfnOutput(this, "EventsTableName", new CfnOutputProps
        {
            Value = EventsTable.TableName,
            ExportName = "CloudOps-EventsTableName"
        });
    }
}