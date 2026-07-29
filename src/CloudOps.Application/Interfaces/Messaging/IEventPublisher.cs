using CloudOps.Domain.Events;

namespace CloudOps.Application.Interfaces.Messaging;

public interface IEventPublisher
{
    Task PublishAsync(EventEnvelope envelope, CancellationToken cancellationToken);
}