using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using CloudOps.Application.Interfaces.Messaging;
using CloudOps.Domain.Events;
using CloudOps.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudOps.Infrastructure.Messaging;

public sealed class AwsSqsEventPublisher : IEventPublisher
{
    private readonly IAmazonSQS _sqs;
    private readonly AwsOptions _options;
    private readonly ILogger<AwsSqsEventPublisher> _logger;

    public AwsSqsEventPublisher(
        IAmazonSQS sqs,
        IOptions<AwsOptions> options,
        ILogger<AwsSqsEventPublisher> logger)
    {
        _sqs = sqs;
        _options = options.Value;
        _logger = logger;
    }

    public async Task PublishAsync(EventEnvelope envelope, CancellationToken cancellationToken)
    {
        var body = JsonSerializer.Serialize(envelope);

        var request = new SendMessageRequest
        {
            QueueUrl = _options.EventQueueUrl,
            MessageBody = body
        };

        var response = await _sqs.SendMessageAsync(request, cancellationToken);

        _logger.LogInformation(
            "Published event {EventId} to SQS with MessageId {MessageId}",
            envelope.EventId,
            response.MessageId);
    }
}