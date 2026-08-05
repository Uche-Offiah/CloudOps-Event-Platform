using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using CloudOps.Application.Interfaces.Monitoring;
using Microsoft.Extensions.Logging;

namespace CloudOps.Infrastructure.Monitoring;

public sealed class CloudWatchMetricPublisher(
    IAmazonCloudWatch cloudWatch,
    ILogger<CloudWatchMetricPublisher> logger)
    : IMetricPublisher
{
    private const string Namespace = "CloudOps/EventPlatform";

    private readonly IAmazonCloudWatch _cloudWatch = cloudWatch;
    private readonly ILogger<CloudWatchMetricPublisher> _logger = logger;

    public async Task PublishCountAsync(
        string metricName,
        double value,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new PutMetricDataRequest
            {
                Namespace = Namespace,

                MetricData =
                [
                    new MetricDatum
                    {
                        MetricName = metricName,

                        Unit = StandardUnit.Count,

                        Value = value,

                        Timestamp = DateTime.UtcNow
                    }
                ]
            };

            await _cloudWatch.PutMetricDataAsync(
                request,
                cancellationToken);
        }
        catch (Exception ex)
        {
            //
            // Metrics must never interrupt event processing.
            //
            _logger.LogWarning(
                ex,
                "Unable to publish CloudWatch metric {MetricName}.",
                metricName);
        }
    }
}