using Amazon.CDK;
using Constructs;
using Infra.Config;
using Infra.Constructs;
using Infra.Common;

namespace Infra.Stacks;

public sealed class CloudOpsPlatformStack : Stack
{
    public CloudOpsPlatformStack(Construct scope, string id, IStackProps? props = null): base(scope, id, props)
    {		
		var config  = new PlatformConfig();
		
		TagHelper.ApplyDefaultTags(this, config);

        _ = new StorageConstruct(this, "Storage", config);

        _ = new MessagingConstruct(this, "Messaging", config);

        _ = new NotificationConstruct(this,"Notifications", config);

        _ = new MonitoringConstruct(this, "Monitoring", config);
    }
}