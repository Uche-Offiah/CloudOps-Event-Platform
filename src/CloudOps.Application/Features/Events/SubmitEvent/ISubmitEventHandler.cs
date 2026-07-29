namespace CloudOps.Application.Features.Events.SubmitEvent;

public interface ISubmitEventHandler
{
    Task<SubmitEventResult> HandleAsync(SubmitEventCommand command, CancellationToken cancellationToken);
}