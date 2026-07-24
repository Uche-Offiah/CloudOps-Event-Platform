using System.Text.Json;
using CloudOps.Application.Interfaces.Persistence;
using CloudOps.Application.Interfaces.Messaging;
using CloudOps.Domain.Events;
using Microsoft.Extensions.Logging;

namespace CloudOps.Application.Features.Events.ProcessEvent;

public sealed class EventMessageProcessor(
    IEventRepository repository,
    ILogger<EventMessageProcessor> logger) : IEventMessageProcessor
{
    private readonly IEventRepository _repository = repository;
    private readonly ILogger<EventMessageProcessor> _logger = logger;

    public async Task<ProcessingResult> ProcessAsync(string messageBody, CancellationToken cancellationToken)
    {
        try
        {
            var envelope = JsonSerializer.Deserialize<EventEnvelope>(messageBody);

            if (envelope is null)
            {
                return ProcessingResult.Failure("Unable to deserialize EventEnvelope.");
            }

            await _repository.SaveAsync( envelope, cancellationToken);

            _logger.LogInformation("Successfully persisted event {EventId}", envelope.EventId);

            return ProcessingResult.Success();
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize EventEnvelope.");

            return ProcessingResult.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while processing event.");

            return ProcessingResult.Failure(ex.Message);
        }
    }
}