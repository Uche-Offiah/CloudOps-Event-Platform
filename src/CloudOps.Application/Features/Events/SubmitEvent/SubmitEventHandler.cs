using System.Text.Json;
using Microsoft.Extensions.Logging;
using CloudOps.Application.Interfaces.Messaging;
using CloudOps.Application.Interfaces.System;
using CloudOps.Domain.Events;

namespace CloudOps.Application.Features.Events.SubmitEvent;

public sealed class SubmitEventHandler : ISubmitEventHandler
{
    private readonly IEventPublisher _publisher;
    private readonly IClock _clock;
    private readonly ICorrelationIdProvider _correlationIdProvider;
    private readonly ILogger _logger;

    public SubmitEventHandler(IEventPublisher publisher, IClock clock, ICorrelationIdProvider correlationIdProvider, ILogger<SubmitEventHandler> logger)
    {
        _publisher = publisher;
        _clock = clock;
        _correlationIdProvider = correlationIdProvider;
        _logger = logger;
    }

    public async Task<SubmitEventResult> HandleAsync(SubmitEventCommand command, CancellationToken cancellationToken)
    {
        SubmitEventValidator.Validate(command);
        
        var eventId = Guid.NewGuid();

        var acceptedAt = _clock.UtcNow;

        var correlationId = _correlationIdProvider.Create();

        using var document = JsonDocument.Parse(command.Payload);

        var envelope = new EventEnvelope(
            eventId,
            command.EventType,
            command.Source,
            acceptedAt,
            correlationId,
            Version:  EventEnvelopeVersions.Initial,
            Payload: document.RootElement.Clone());

            _logger.LogInformation("Submitting event {EventId} ({EventType}) from {Source} with CorrelationId {CorrelationId}",
            eventId,
            command.EventType,
            command.Source,
            correlationId);

        await _publisher.PublishAsync(envelope, cancellationToken);

        return new SubmitEventResult(eventId, acceptedAt);
    }
}