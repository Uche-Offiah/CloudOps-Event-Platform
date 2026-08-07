using Amazon.CDK;
using Constructs;
using CloudOps.Infrastructure.Cdk.Configuration;

namespace CloudOps.Infrastructure.Cdk.Common;

public static class TagHelper
{
    public static void ApplyDefaultTags(
        Construct construct,
        PlatformConfiguration config)
    {
        foreach (var tag in config.Tags)
        {
            Amazon.CDK.Tags.Of(construct)
                .Add(tag.Key, tag.Value);
        }
    }
}