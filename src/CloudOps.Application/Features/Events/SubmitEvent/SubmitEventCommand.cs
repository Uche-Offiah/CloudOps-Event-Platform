namespace CloudOps.Application.Features.Events.SubmitEvent;

public sealed record SubmitEventCommand(string Source, string EventType, string Payload);