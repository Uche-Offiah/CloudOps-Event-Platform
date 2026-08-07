using Amazon.CDK.AWS.CloudWatch;

namespace CloudOps.Infrastructure.Cdk.Models;

public sealed record MonitoringResources(Dashboard Dashboard, Alarm EventsFailedAlarm,
    Alarm DuplicateEventsAlarm);