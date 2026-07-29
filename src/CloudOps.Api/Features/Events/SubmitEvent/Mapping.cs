using CloudOps.Application.Features.Events.SubmitEvent;
using CloudOps.Contracts.Events;

namespace CloudOps.Api.Features.Events.SubmitEvent;

internal static class Mapping
{
    public static SubmitEventCommand ToCommand(this SubmitEventRequest request)
    {
        return new SubmitEventCommand(request.Source, request.EventType, request.Payload);
    }

    public static SubmitEventResponse ToResponse(this SubmitEventResult result)
    {
        return new SubmitEventResponse(result.EventId, result.AcceptedAtUtc, "Accepted");
    }
}