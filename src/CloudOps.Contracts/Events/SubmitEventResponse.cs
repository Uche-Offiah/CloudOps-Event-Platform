namespace CloudOps.Contracts.Events;

public sealed record SubmitEventResponse(Guid EventId, DateTimeOffset AcceptedAtUtc, string Status);