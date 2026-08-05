using Amazon.SQS;
using Amazon.DynamoDBv2;
using CloudOps.Application.Interfaces.Messaging;
using CloudOps.Application.Interfaces.Persistence;
using CloudOps.Application.Interfaces.Monitoring;
using CloudOps.Application.Interfaces.System;
using CloudOps.Infrastructure.Configuration;
using CloudOps.Infrastructure.Messaging;
using CloudOps.Infrastructure.System;
using CloudOps.Infrastructure.Monitoring;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CloudOps.Infrastructure.Persistence;
using Amazon.CloudWatch;

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
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddAWSService<IAmazonCloudWatch>();

        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<ICorrelationIdProvider, CorrelationIdProvider>();
        services.AddSingleton<IMetricPublisher, CloudWatchMetricPublisher>();

        services.AddScoped<IEventPublisher, AwsSqsEventPublisher>();
        services.AddScoped<IEventRepository, DynamoDbEventRepository>();

        return services;
    }
}