using CloudOps.Application.Features.Events.ProcessEvent;
using CloudOps.Application.Features.Events.SubmitEvent;
using Microsoft.Extensions.DependencyInjection;

namespace CloudOps.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ISubmitEventHandler, SubmitEventHandler>();
        services.AddScoped<IEventMessageProcessor, EventMessageProcessor>();

        return services;
    }
}