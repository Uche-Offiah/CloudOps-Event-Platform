using Amazon.CDK;
using Amazon.CDK.Assertions;
using CloudOps.Infrastructure.Cdk.Stacks;
using Xunit;

namespace CloudOps.Infrastructure.Cdk.Tests.Stacks;

public sealed class PlatformStackTests
{
    [Fact]
    public void PlatformStack_Should_Synthesize()
    {
        var app = new App();

        var stack = new PlatformStack(app, "PlatformStack", new StackProps());

        var template = Template.FromStack(stack);

        Assert.NotNull(template);
    }

    [Fact]
    public void PlatformStack_Should_Create_EventTable()
    {
        var app = new App();

        var stack = new PlatformStack(app, "PlatformStack", new StackProps());

        var template = Template.FromStack(stack);

        template.ResourceCountIs("AWS::DynamoDB::Table", 1);
    }

    [Fact]
    public void PlatformStack_Should_Create_EventQueues()
    {
        var app = new App();

        var stack = new PlatformStack(app, "PlatformStack", new StackProps());

        var template = Template.FromStack(stack);

        template.ResourceCountIs("AWS::SQS::Queue", 2);
    }

    [Fact]
    public void PlatformStack_Should_Create_AlertsTopic()
    {
        var app = new App();

        var stack = new PlatformStack(app, "PlatformStack", new StackProps());

        var template = Template.FromStack(stack);

        template.ResourceCountIs("AWS::SNS::Topic", 1);
    }

    [Fact]
    public void PlatformStack_Should_Create_Dashboard()
    {
        var app = new App();

        var stack = new PlatformStack(app,"PlatformStack",new StackProps());

        var template = Template.FromStack(stack);

        template.ResourceCountIs( "AWS::CloudWatch::Dashboard", 1);
    }

    [Fact]
    public void PlatformStack_Should_Create_Alarms()
    {
        var app = new App();

        var stack = new PlatformStack( app, "PlatformStack", new StackProps());

        var template = Template.FromStack(stack);

        template.ResourceCountIs("AWS::CloudWatch::Alarm", 2);
    }
}