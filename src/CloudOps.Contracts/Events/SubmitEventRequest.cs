namespace CloudOps.Contracts.Events;

public sealed record SubmitEventRequest(string Source, string EventType, string Payload);