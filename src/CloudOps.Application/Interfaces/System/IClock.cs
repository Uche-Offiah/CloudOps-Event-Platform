namespace CloudOps.Application.Interfaces.System;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}