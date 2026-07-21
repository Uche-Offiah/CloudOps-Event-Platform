using Amazon.CDK;
using Constructs;
using Infra.Config;

namespace Infra.Common;

public static class TagHelper
{
    public static void ApplyDefaultTags(
        Construct construct,
        PlatformConfig config)
    {
        foreach (var tag in config.Tags)
        {
            Amazon.CDK.Tags.Of(construct)
                .Add(tag.Key, tag.Value);
        }
    }
}