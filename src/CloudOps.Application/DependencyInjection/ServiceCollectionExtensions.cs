using CloudOps.Application.Features.Events.ProcessEvent;
using CloudOps.Application.Features.Events.SubmitEvent;
using CloudOps.Application.Interfaces.Messaging;
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