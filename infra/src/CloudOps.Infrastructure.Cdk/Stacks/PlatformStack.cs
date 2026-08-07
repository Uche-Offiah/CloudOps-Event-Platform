using Amazon.CDK;
using Constructs;
using CloudOps.Infrastructure.Cdk.Configuration;
using CloudOps.Infrastructure.Cdk.Constructs;
using CloudOps.Infrastructure.Cdk.Constructs.Monitoring;
using CloudOps.Infrastructure.Cdk.Constructs.Messaging;
using CloudOps.Infrastructure.Cdk.Constructs.Storage;
using CloudOps.Infrastructure.Cdk.Common;

namespace CloudOps.Infrastructure.Cdk.Stacks;

public sealed class PlatformStack : Stack
{
    public PlatformStack(Construct scope, string id, IStackProps? props = null): base(scope, id, props)
    {		
		var config  = new PlatformConfiguration();
		
		TagHelper.ApplyDefaultTags(this, config);

        _ = new StorageConstruct(this, "Storage", config);

        _ = new MessagingConstruct(this, "Messaging", config);

        var notifications = new NotificationConstruct(this,"Notifications", config);

        _ = new MonitoringConstruct(this, "Monitoring", config, notifications.Resources.AlertsTopic);
    }
}