namespace CloudOps.Infrastructure.Configuration;

public sealed class AwsOptions
{
    public const string SectionName = "Aws";

    public required string Region { get; init; }

    public required string EventQueueUrl { get; init; }
    public required string EventsTableName { get; init; }
}