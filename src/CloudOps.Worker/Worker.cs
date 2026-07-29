using Amazon.SQS;
using Amazon.SQS.Model;
using CloudOps.Infrastructure.Configuration;
using CloudOps.Application.Interfaces.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudOps.Worker;

public sealed class Worker: BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly AwsOptions _awsOptions;
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    //private readonly IEventMessageProcessor _messageProcessor;

    public Worker(IAmazonSQS sqs, IOptions<AwsOptions> awsOptions, ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
    {
        _sqs = sqs;
        _awsOptions = awsOptions.Value;
        _logger = logger;
        _scopeFactory = scopeFactory;
        //_messageProcessor =  messageProcessor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        
        _logger.LogInformation("CloudOps Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {

            using var scope = _scopeFactory.CreateScope();
            var processor = scope.ServiceProvider.GetRequiredService<IEventMessageProcessor>();
            
            try
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _awsOptions.EventQueueUrl,
                    MaxNumberOfMessages = 5,
                    WaitTimeSeconds = 20,
                    VisibilityTimeout = 30
                };

                var response = await _sqs.ReceiveMessageAsync(request, stoppingToken);

                var messages = response.Messages ?? [];

                if (messages.Count == 0)
                {
                    _logger.LogDebug("No messages available.");
                    continue;
                }

                _logger.LogInformation("Received {Count} message(s).", messages.Count);

                foreach (var message in messages)
                {
                    _logger.LogInformation("Processing MessageId: {MessageId}", message.MessageId);
                    
                    var result  = await processor.ProcessAsync(message.Body, stoppingToken);

                     if (!result.Succeeded)
                    {
                        _logger.LogWarning(
                            "Processing failed for MessageId {MessageId}. Reason: {Reason}",
                            message.MessageId,
                            result.FailureReason);

                        continue;
                    }

                    await _sqs.DeleteMessageAsync(
                        _awsOptions.EventQueueUrl,
                        message.ReceiptHandle,
                        stoppingToken);

                    _logger.LogInformation(
                        "Deleted MessageId {MessageId} from SQS.",
                        message.MessageId);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while polling Amazon SQS");
            }
        }

        _logger.LogInformation("CloudOps Worker stopped.");
    }
}
