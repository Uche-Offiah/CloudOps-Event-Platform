# ADR-0009: Event Processing Pipeline Architecture

- **Status:** Accepted
- **Date:** 2026-07-29
- **Category:** Processing
- **Milestone:** 3
- **Decision Makers:** Project Owner

---

## Context

After establishing asynchronous messaging and introducing a dedicated Worker service, the platform required a well-defined processing pipeline that consistently transformed accepted events into persisted records.

The processing workflow needed to:

- Preserve event integrity.
- Support idempotent persistence.
- Isolate business logic from infrastructure concerns.
- Handle processing failures predictably.
- Enable operational visibility across each processing stage.

The architecture also needed to support future enhancements such as retries, dead-letter queues, replay, enrichment, and additional downstream consumers without requiring fundamental redesign.

---

## Decision

The platform adopts a staged event processing pipeline.

Each message progresses through a fixed sequence of responsibilities:

1. Receive a message from Amazon SQS.
2. Deserialize the canonical event envelope.
3. Validate the message structure.
4. Execute application processing.
5. Persist the event.
6. Publish operational metrics.
7. Emit structured logs.
8. Delete the message from the queue only after successful completion.

Each stage has a single responsibility and communicates through the Application layer.

---

## Architectural Principles

### Pipeline Responsibility

Each processing stage performs one clearly defined responsibility before passing control to the next stage.

### Idempotent Processing

Persistence is protected against duplicate writes by enforcing conditional writes using the event identifier as the uniqueness constraint.

Duplicate messages are treated as expected operational scenarios rather than application failures.

### Failure Isolation

Failures stop processing for the current message only.

The message remains in Amazon SQS for future retry according to queue visibility timeout and retry policies.

### Infrastructure Separation

Business processing remains independent of AWS SDK implementations.

Infrastructure concerns are isolated behind abstractions provided by the Infrastructure layer.

### Observability

Every stage produces structured logs and publishes operational metrics to enable end-to-end monitoring and troubleshooting.

---

## Rationale

A staged pipeline provides predictable processing behavior while maintaining clear separation of concerns.

This architecture simplifies:

- Testing.
- Troubleshooting.
- Future pipeline extension.
- Operational monitoring.
- Maintenance.

Each processing stage may evolve independently provided the overall pipeline contract remains unchanged.

---

## Consequences

### Benefits

- Predictable processing flow.
- Clear separation of responsibilities.
- Improved maintainability.
- Built-in support for idempotency.
- Easier operational diagnostics.
- Simplified future enhancements.

### Trade-offs

- More application components than a single processing method.
- Additional coordination between processing stages.
- Increased emphasis on structured logging and monitoring.

---

## Alternatives Considered

### Single Processing Method

Rejected.

Combining deserialization, validation, persistence, logging, and infrastructure interactions into one method reduces readability, increases coupling, and complicates testing.

### Workflow Engine

Rejected.

Dedicated workflow engines provide advanced orchestration but introduce unnecessary complexity for the current scope of the platform.

### Database-First Processing

Rejected.

Persisting raw messages before application processing would complicate business validation and increase coupling between storage and processing responsibilities.

---

## Implementation Summary

As of **Milestone 4 (v0.4.0)**, the processing pipeline performs the following sequence:

1. Worker receives messages using Amazon SQS long polling.
2. A scoped dependency injection container is created for the processing cycle.
3. The Application layer deserializes the canonical event envelope.
4. The event is validated.
5. The repository persists the event to Amazon DynamoDB using conditional writes.
6. Duplicate events are detected through DynamoDB conditional write failures and recorded as operational events.
7. CloudWatch custom metrics are published for successful, duplicate, and failed processing outcomes.
8. Structured logging maintains correlation across the complete processing lifecycle.
9. Successfully processed messages are deleted from Amazon SQS.

This implementation has demonstrated clear separation between messaging, application orchestration, persistence, and observability while preserving Clean Architecture boundaries.

---

## Related ADRs

- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0004 – Clean Architecture and Vertical Slice Organization
- ADR-0006 – Canonical Event Envelope
- ADR-0007 – Dedicated Worker Service
- ADR-0008 – Amazon SQS Long Polling Strategy
- ADR-0010 – Observability Strategy

---

## Status

This decision remains valid as of **Milestone 4 (v0.4.0)** and continues to define the event processing architecture of the CloudOps Event Platform.