using Amazon.SQS;
using Amazon.SQS.Model;
using CloudOps.Infrastructure.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudOps.Worker;

public sealed class Worker: BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly AwsOptions _awsOptions;
    private readonly ILogger<Worker> _logger;

    public Worker(IAmazonSQS sqs, IOptions<AwsOptions> awsOptions, ILogger<Worker> logger)
    {
        _sqs = sqs;
        _awsOptions = awsOptions.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CloudOps Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Creating ReceiveMessageRequest...");

                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _awsOptions.EventQueueUrl,
                    MaxNumberOfMessages = 5,
                    WaitTimeSeconds = 20,
                    VisibilityTimeout = 30
                };

                _logger.LogInformation("ReceiveMessageAsync completed.");

                var response = await _sqs.ReceiveMessageAsync(request, stoppingToken);

                var messages = response.Messages ?? [];

                if (messages.Count == 0)
                {
                    _logger.LogDebug("No messages available.");
                    continue;
                }

                // _logger.LogInformation("Received {Count} message(s).", messages.Count);
                // _logger.LogInformation("ReceiveMessageAsync completed.");
                _logger.LogInformation("Received {Count} message(s).", messages.Count);

                foreach (var message in messages)
                {
                    //_logger.LogInformation("About to log MessageId.");
                    _logger.LogInformation("MessageId: {MessageId}", message.MessageId);
                    _logger.LogInformation("Finished processing message.");
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
                    "Unexpected error while polling Amazon SQS. StackTrace: {StackTrace}", ex.StackTrace);
            }
        }

        _logger.LogInformation("CloudOps Worker stopped.");
    }
}
