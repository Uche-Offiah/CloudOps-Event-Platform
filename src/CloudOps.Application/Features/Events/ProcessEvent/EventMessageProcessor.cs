using System.Text.Json;
using CloudOps.Domain.Events;
using Microsoft.Extensions.Logging;

namespace CloudOps.Application.Features.Events.ProcessEvent;

public sealed class EventMessageProcessor(ILogger<EventMessageProcessor> logger): IEventMessageProcessor
{
    private readonly ILogger<EventMessageProcessor> _logger = logger;

    public Task<ProcessingResult> ProcessAsync( string messageBody, CancellationToken cancellationToken)
    {
        try
        {
            var envelope = JsonSerializer.Deserialize<EventEnvelope>(messageBody);

            if (envelope is null)
            {
                return Task.FromResult(ProcessingResult.Failure("Unable to deserialize EventEnvelope."));
            }

            _logger.LogInformation(
                "Successfully deserialized event {EventId} ({EventType})",
                envelope.EventId,
                envelope.EventType
            );


            // Persistence will be introduced subsequently.

            return Task.FromResult(ProcessingResult.Success());
        }
        catch (JsonException ex)
        {
            _logger.LogError(
                ex,
                "Failed to deserialize EventEnvelope.");

            return Task.FromResult(ProcessingResult.Failure(ex.Message));
        }
    }
}