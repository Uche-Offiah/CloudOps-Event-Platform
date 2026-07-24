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
    // protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    // {
    //     while (!stoppingToken.IsCancellationRequested)
    //     {
    //         logger.LogInformation("Worker heartbeat at {Time}", DateTimeOffset.UtcNow);
    //         // if (logger.IsEnabled(LogLevel.Information))
    //         // {
    //         //     logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
    //         // }
    //         await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
    //     }
    // }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CloudOps Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
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

                _logger.LogInformation(
                    "Received {Count} message(s).",
                    response.Messages.Count);

                foreach (var message in response.Messages)
                {
                    _logger.LogInformation(
                        "MessageId: {MessageId}",
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
                    "Unexpected error while polling Amazon SQS.");
            }
        }

        _logger.LogInformation("CloudOps Worker stopped.");
    }
}
