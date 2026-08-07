using Amazon.CDK;
using Amazon.CDK.Assertions;
using CloudOps.Infrastructure.Cdk.Stacks;
using Xunit;

namespace CloudOps.Infrastructure.Cdk.Tests.Stacks;

public class FoundationStackTests
{
    [Fact]
    public void FoundationStack_Should_Synthesize()
    {
        var app = new App();

        var stack = new FoundationStack(app, "FoundationStack", new StackProps());

        var template = Template.FromStack(stack);

        Assert.NotNull(template);
    }

    // For VPC test
    [Fact]
    public void FoundationStack_Should_Create_Vpc()
    {
        var app = new App();

        var stack = new FoundationStack(app, "Foundation");

        var template = Template.FromStack(stack);

        template.ResourceCountIs("AWS::EC2::VPC", 1);
    }

    // For SQS test
    [Fact]
    public void PlatformStack_Should_Create_EventQueue()
    {
        var app = new App();

        var stack = new PlatformStack(app, "Platform");

        var template = Template.FromStack(stack);

        template.ResourceCountIs("AWS::SQS::Queue", 2);
    }

    // For DynamoDB test
    [Fact]
    public void PlatformStack_Should_Create_EventTable()
    {
        var app = new App();

        var stack = new PlatformStack(app, "Platform");

        var template = Template.FromStack(stack);

        template.HasResourceProperties(
            "AWS::DynamoDB::Table",
            new Dictionary<string, object>
            {
                ["BillingMode"] = "PAY_PER_REQUEST"
            });
    }

}