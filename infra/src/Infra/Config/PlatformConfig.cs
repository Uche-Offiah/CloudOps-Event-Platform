namespace Infra.Config;
using System.Collections.Generic;

public sealed class PlatformConfig
{
    public string ApplicationName { get; init; } = "cloudops";

    public string PlatformName { get; init; } = "event-platform";

    public string Environment { get; init; } = "dev";

    public string Owner { get; init; } = "YourGitHubUsername";

    public Dictionary<string, string> Tags =>
        new()
        {
            ["Application"] = ApplicationName,
            ["Platform"] = PlatformName,
            ["Environment"] = Environment,
            ["ManagedBy"] = "AWS CDK",
            ["Owner"] = Owner,
            ["Project"] = "CloudOps Event Platform",
            ["Repository"] = "CloudOps-Event-Platform",
            ["Provisioner"] = "AWS CDK",
            ["Version"] = "1.0.0"
        };
}