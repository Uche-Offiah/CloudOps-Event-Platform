using System.Text.Json;

namespace CloudOps.Domain.Events;

public sealed record EventEnvelope(
    Guid EventId,
    string EventType,
    string Source,
    DateTimeOffset OccurredAtUtc,
    Guid CorrelationId,
    int Version,
    JsonElement Payload);