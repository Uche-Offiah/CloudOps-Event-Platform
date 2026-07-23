using CloudOps.Api.Features.Events.SubmitEvent;

namespace CloudOps.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapApi(
        this IEndpointRouteBuilder app)
    {
        app.MapSubmitEvent();

        return app;
    }
}