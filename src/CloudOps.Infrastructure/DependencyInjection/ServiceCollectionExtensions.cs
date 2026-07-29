using Amazon.SQS;
using CloudOps.Application.Interfaces.Messaging;
using CloudOps.Application.Interfaces.System;
using CloudOps.Infrastructure.Configuration;
using CloudOps.Infrastructure.Messaging;
using CloudOps.Infrastructure.System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudOps.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AwsOptions>(
            configuration.GetSection(AwsOptions.SectionName));

        services.AddAWSService<IAmazonSQS>();

        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<ICorrelationIdProvider, CorrelationIdProvider>();

        services.AddScoped<IEventPublisher, AwsSqsEventPublisher>();

        return services;
    }
}