# ADR-0005: Event Ingestion API Design

## Status

Accepted

## Context

The platform requires an HTTP endpoint that accepts operational events and forwards them to the asynchronous processing pipeline.

The endpoint should remain lightweight and avoid performing long-running work.

## Decision

The API returns **HTTP 202 Accepted** after successfully validating the request and publishing an event to Amazon SQS.

Processing is intentionally asynchronous.

The API does not:

- Persist events
- Perform business processing
- Wait for downstream consumers

## Consequences

### Positive

- Low response latency
- Better scalability
- Loose coupling between producers and consumers
- Supports retry and dead-letter strategies

### Negative

- Clients cannot assume processing completed successfully.
- Additional monitoring is required for asynchronous workflows.

## Alternatives Considered

### Synchronous persistence

Rejected because it tightly couples the API to downstream storage and increases request latency.

### Direct DynamoDB writes

Rejected because it bypasses the event-driven architecture and makes future integrations more difficult.