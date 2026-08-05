using Amazon.CDK.AWS.CloudWatch;

namespace Infra.Models;

public sealed record MonitoringResources(Dashboard Dashboard, Alarm EventsFailedAlarm,
    Alarm DuplicateEventsAlarm);