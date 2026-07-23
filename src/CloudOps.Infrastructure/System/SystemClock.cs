using CloudOps.Application.Interfaces.System;

namespace CloudOps.Infrastructure.System;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}