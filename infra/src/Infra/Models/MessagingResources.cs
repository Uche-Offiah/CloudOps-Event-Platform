using Amazon.CDK.AWS.SQS;

namespace Infra.Models;

public sealed record MessagingResources(Queue EventQueue, Queue DeadLetterQueue);