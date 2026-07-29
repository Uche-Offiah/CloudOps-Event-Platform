using CloudOps.Application.Interfaces.System;

namespace CloudOps.Infrastructure.System;

public sealed class CorrelationIdProvider : ICorrelationIdProvider
{
    public Guid Create() => Guid.NewGuid();
}