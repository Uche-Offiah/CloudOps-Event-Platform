using Amazon.CDK.AWS.SQS;

namespace CloudOps.Infrastructure.Cdk.Models;

public sealed record MessagingResources(Queue EventQueue, Queue DeadLetterQueue);