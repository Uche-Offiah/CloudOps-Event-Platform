using CloudOps.Worker;
using Amazon;
using Amazon.SQS;
using CloudOps.Application.DependencyInjection;
using CloudOps.Infrastructure.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

// Shared application registrations
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// AWS SDK
builder.Services.AddSingleton<IAmazonSQS>(_ =>
    new AmazonSQSClient(RegionEndpoint.USEast2));

// Worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
