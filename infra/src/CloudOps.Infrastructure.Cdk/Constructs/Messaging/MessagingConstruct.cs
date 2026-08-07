using Amazon.CDK;
using Amazon.CDK.AWS.SQS;
using Constructs;
using CloudOps.Infrastructure.Cdk.Common;
using CloudOps.Infrastructure.Cdk.Configuration;
using CloudOps.Infrastructure.Cdk.Models;

namespace CloudOps.Infrastructure.Cdk.Constructs.Messaging;

public sealed class MessagingConstruct : Construct
{
    public MessagingResources Resources { get; }

    public MessagingConstruct(Construct scope, string id, PlatformConfiguration config): base(scope, id)
    {
        var deadLetterQueue = new Queue(this, "DeadLetterQueue", new QueueProps
        {
            QueueName = ResourceNaming.DeadLetterQueue(config),

            RetentionPeriod = Duration.Days(14),

            Encryption = QueueEncryption.SQS_MANAGED,

            RemovalPolicy = RemovalPolicy.DESTROY
        });

        var eventQueue = new Queue(this, "EventQueue", new QueueProps
        {
            QueueName = ResourceNaming.EventQueue(config),

            VisibilityTimeout = Duration.Seconds(30),

            RetentionPeriod = Duration.Days(4),

            Encryption = QueueEncryption.SQS_MANAGED,

            DeadLetterQueue = new DeadLetterQueue
            {
                Queue = deadLetterQueue,
                MaxReceiveCount = 5
            },

            RemovalPolicy = RemovalPolicy.DESTROY
        });

        new CfnOutput(this, "EventQueueUrl", new CfnOutputProps
        {
            Value = eventQueue.QueueUrl,
            Description = "Primary event queue URL"
        });

        new CfnOutput(this, "DeadLetterQueueUrl", new CfnOutputProps
        {
            Value = deadLetterQueue.QueueUrl,
            Description = "Dead Letter Queue URL"
        });

        Resources = new MessagingResources(eventQueue,deadLetterQueue);
    }
}