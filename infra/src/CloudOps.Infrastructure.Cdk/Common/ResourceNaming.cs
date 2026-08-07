using CloudOps.Infrastructure.Cdk.Configuration;

namespace CloudOps.Infrastructure.Cdk.Common;

public static class ResourceNaming
{
    public static string EventsTable(PlatformConfiguration config)
        => $"{config.ApplicationName}-{config.Environment}-events";

    public static string EventQueue(PlatformConfiguration config)
        => $"{config.ApplicationName}-{config.Environment}-event-queue";

    public static string DeadLetterQueue(PlatformConfiguration config)
        => $"{config.ApplicationName}-{config.Environment}-event-dlq";

    public static string AlertsTopic(PlatformConfiguration config)
        => $"{config.ApplicationName}-{config.Environment}-alerts";

    public static string DashboardName(PlatformConfiguration config)
        => $"{config.ApplicationName}-{config.Environment}-dashboard";
}