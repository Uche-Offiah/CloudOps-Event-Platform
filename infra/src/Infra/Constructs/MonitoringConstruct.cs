using Amazon.CDK.AWS.CloudWatch;
using Constructs;
using Infra.Common;
using Infra.Config;
using Infra.Models;

namespace Infra.Constructs;

public sealed class MonitoringConstruct : Construct
{
    public MonitoringResources Resources { get; }

    public MonitoringConstruct(Construct scope, string id, PlatformConfig config): base(scope, id)
    {
        var dashboard = new Dashboard(this, "Dashboard", new DashboardProps
        {
            DashboardName = ResourceNaming.DashboardName(config)
        });

        new Amazon.CDK.CfnOutput(this,"DashboardName", new Amazon.CDK.CfnOutputProps
        {
            Value = dashboard.DashboardName
        });

        Resources = new MonitoringResources(dashboard);
    }
}