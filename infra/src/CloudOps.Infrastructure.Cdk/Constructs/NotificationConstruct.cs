using Amazon.CDK;
using Amazon.CDK.AWS.SNS;
using Constructs;
using CloudOps.Infrastructure.Cdk.Common;
using CloudOps.Infrastructure.Cdk.Configuration;
using CloudOps.Infrastructure.Cdk.Models;

namespace CloudOps.Infrastructure.Cdk.Constructs;

public sealed class NotificationConstruct : Construct
{
    public NotificationResources Resources { get; }

    public NotificationConstruct(Construct scope, string id, PlatformConfiguration config): base(scope, id)
    {
        var topic = new Topic(this, "AlertsTopic", new TopicProps
        {
            TopicName = ResourceNaming.AlertsTopic(config),

            MasterKey = null
        });

        new CfnOutput(this, "AlertsTopicArn", new CfnOutputProps
        {
            Value = topic.TopicArn,
            Description = "CloudOps Alerts SNS Topic ARN"
        });

        Resources = new NotificationResources(topic);
    }
}