using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using CloudOps.Application.Interfaces.Persistence;
using CloudOps.Domain.Events;
using CloudOps.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudOps.Infrastructure.Persistence;

public sealed class DynamoDbEventRepository(
    IAmazonDynamoDB dynamoDb, IOptions<AwsOptions> awsOptions, ILogger<DynamoDbEventRepository> logger) : IEventRepository
{
    private readonly IAmazonDynamoDB _dynamoDb = dynamoDb;
    private readonly AwsOptions _awsOptions = awsOptions.Value;
    private readonly ILogger<DynamoDbEventRepository> _logger = logger;

    public async Task SaveAsync(EventEnvelope envelope, CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(
            new Dictionary<string, object?>
            {
                ["EventId"] = envelope.EventId,
                ["CorrelationId"] = envelope.CorrelationId,
                ["EventType"] = envelope.EventType,
                ["Source"] = envelope.Source,
                ["TableName"] = _awsOptions.EventsTableName
            }
        );
        var request = new PutItemRequest
        {
            TableName = _awsOptions.EventsTableName,

            ConditionExpression = "attribute_not_exists(EventId)",

            Item = new Dictionary<string, AttributeValue>
            {
                ["EventId"] = new()
                {
                    S = envelope.EventId.ToString()
                },

                ["EventType"] = new()
                {
                    S = envelope.EventType
                },

                ["Source"] = new()
                {
                    S = envelope.Source
                },

                ["OccurredAtUtc"] = new()
                {
                    S = envelope.OccurredAtUtc.ToString("O")
                },

                ["CorrelationId"] = new()
                {
                    S = envelope.CorrelationId.ToString()
                },

                ["Version"] = new()
                {
                    N = envelope.Version.ToString()
                },

                ["Payload"] = new()
                {
                    S = JsonSerializer.Serialize(envelope.Payload)
                }
            }
        };


        try
        {
            _logger.LogInformation("Persisting event to DynamoDB");

            await _dynamoDb.PutItemAsync(request, cancellationToken);

            _logger.LogInformation("Event successfully persisted");
        }
        catch (ConditionalCheckFailedException ex)
        {
            _logger.LogWarning(ex, "Duplicate EventId detected.");

            throw;
        }

        _logger.LogInformation("Successfully persisted event");
    }
}