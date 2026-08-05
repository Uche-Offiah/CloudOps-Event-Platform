
namespace CloudOps.Application.Interfaces.Monitoring;

public interface IMetricPublisher
{
    Task PublishCountAsync(string metricName, double value, CancellationToken cancellationToken);
}