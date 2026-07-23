using CloudOps.Application.Features.Events.SubmitEvent;
using CloudOps.Contracts.Events;

namespace CloudOps.Api.Features.Events.SubmitEvent;

public static class Endpoint
{
    public static IEndpointRouteBuilder MapSubmitEvent( this IEndpointRouteBuilder app)
    {
        app.MapPost("/events", async (SubmitEventRequest request, ISubmitEventHandler handler, CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(request.ToCommand(), cancellationToken);

                //return Results.Accepted(value: result.ToResponse());
                return Results.Accepted($"/events/{result.EventId}", result);
            });

        return app;
    }
}