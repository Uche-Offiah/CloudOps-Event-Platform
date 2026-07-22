using Amazon.CDK.AWS.SNS;

namespace Infra.Models;

public sealed record NotificationResources(Topic AlertsTopic);