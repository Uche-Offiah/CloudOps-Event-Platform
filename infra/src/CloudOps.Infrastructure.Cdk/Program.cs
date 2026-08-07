
using Amazon.CDK;
using CloudOps.Infrastructure.Cdk.Stacks;

var app = new App();

new PlatformStack(app, "PlatformStack",
    new StackProps
    {
        Env = new Amazon.CDK.Environment
        {
            Account = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
            Region = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_REGION")
        }
    });

app.Synth();