using Amazon.CDK.AWS.SNS;

namespace CloudOps.Infrastructure.Cdk.Models;

public sealed record NotificationResources(Topic AlertsTopic);