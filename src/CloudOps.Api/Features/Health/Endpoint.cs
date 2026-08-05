using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CloudOps.Api.Features.Health;

public static class Endpoint
{
    public static IEndpointRouteBuilder MapHealth(
        this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () =>
        {
            return Results.Ok(new
            {
                Status = "Healthy",
                Service = "CloudOps Event Platform API",
                Version = "v0.4.0",
                TimestampUtc = DateTime.UtcNow
            });
        })
        .WithName("Health")
        .WithSummary("Health Check")
        .WithDescription("Returns the operational status of the CloudOps API.")
        .Produces(StatusCodes.Status200OK);

        return app;
    }
}