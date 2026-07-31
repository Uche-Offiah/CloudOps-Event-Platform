using CloudOps.Api.Features.Events.SubmitEvent;
using CloudOps.Api.Features.Health;

namespace CloudOps.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapApi(
        this IEndpointRouteBuilder app)
    {
        app.MapSubmitEvent();
        app.MapHealth();

        return app;
    }
}