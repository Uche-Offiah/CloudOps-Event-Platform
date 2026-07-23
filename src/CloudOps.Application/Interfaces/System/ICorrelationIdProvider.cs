namespace CloudOps.Application.Interfaces.System;

public interface ICorrelationIdProvider
{
    Guid Create();
}