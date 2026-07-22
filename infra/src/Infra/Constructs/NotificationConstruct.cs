using Amazon.CDK;
using Amazon.CDK.AWS.SNS;
using Constructs;
using Infra.Common;
using Infra.Config;
using Infra.Models;

namespace Infra.Constructs;

public sealed class NotificationConstruct : Construct
{
    public NotificationResources Resources { get; }

    public NotificationConstruct(Construct scope, string id, PlatformConfig config): base(scope, id)
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