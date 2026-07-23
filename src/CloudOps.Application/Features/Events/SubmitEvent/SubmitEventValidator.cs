namespace CloudOps.Application.Features.Events.SubmitEvent;

public static class SubmitEventValidator
{
    public static void Validate(SubmitEventCommand command)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Source);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.EventType);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Payload);
    }
}