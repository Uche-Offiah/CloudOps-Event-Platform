namespace CloudOps.Application.Features.Events.SubmitEvent;

public sealed record SubmitEventResult(Guid EventId, DateTimeOffset AcceptedAtUtc);