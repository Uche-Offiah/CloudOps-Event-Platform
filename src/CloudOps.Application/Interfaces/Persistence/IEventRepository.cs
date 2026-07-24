using CloudOps.Domain.Events;

namespace CloudOps.Application.Interfaces.Persistence;

public interface IEventRepository
{
    Task SaveAsync(EventEnvelope envelope, CancellationToken cancellationToken);
}