using Infra.Config;

namespace Infra.Common;

public static class ResourceNaming
{
    public static string EventsTable(PlatformConfig config)
        => $"{config.ApplicationName}-{config.Environment}-events";

    public static string EventQueue(PlatformConfig config)
        => $"{config.ApplicationName}-{config.Environment}-event-queue";

    public static string DeadLetterQueue(PlatformConfig config)
        => $"{config.ApplicationName}-{config.Environment}-event-dlq";

    public static string AlertsTopic(PlatformConfig config)
        => $"{config.ApplicationName}-{config.Environment}-alerts";
}