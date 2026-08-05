# ADR-0003: Asynchronous Event Processing with Amazon SQS

- **Status:** Accepted
- **Date:** 2026-07-22
- **Category:** Messaging
- **Milestone:** 1
- **Decision Makers:** Project Owner

---

## Context

The CloudOps Event Platform accepts operational events through an HTTP API and processes them asynchronously before persisting them.

The platform requires an architecture that:

- Decouples event producers from event consumers.
- Supports independent scaling of API and processing components.
- Improves resilience during temporary downstream failures.
- Buffers bursts of incoming traffic.
- Enables future integration with additional event consumers.

Synchronous processing would tightly couple the API to downstream persistence and reduce overall system resilience.

---

## Decision

The platform will adopt an asynchronous messaging architecture using **Amazon Simple Queue Service (Amazon SQS)**.

The API is responsible for validating and publishing events to an SQS queue.

A dedicated Worker service consumes messages from the queue, processes them, and persists the canonical event envelope.

This establishes a producer-consumer architecture in which producers and consumers evolve independently.

---

## Architectural Principles

### Loose Coupling

The API has no knowledge of how events are ultimately processed or persisted.

### Asynchronous Communication

Requests are accepted immediately while processing occurs independently.

### Durable Messaging

Events remain available in the queue until successfully processed or moved to a Dead Letter Queue.

### Independent Scalability

API instances and Worker instances may scale independently based on workload.

### Failure Isolation

Temporary failures in downstream processing do not prevent the API from accepting new events.

---

## Rationale

Amazon SQS provides a managed, highly available messaging service that integrates naturally with AWS-native architectures.

Using SQS allows the platform to:

- Improve responsiveness.
- Increase resilience.
- Simplify scaling.
- Decouple application layers.
- Support future event-driven workflows.

The design also provides a foundation for introducing retries, dead-letter queues, replay capabilities, and additional consumers in future milestones.

---

## Consequences

### Benefits

- Improved fault tolerance.
- Better response times for API clients.
- Reduced coupling between services.
- Independent deployment of API and Worker.
- Simplified horizontal scaling.
- Durable event delivery.

### Trade-offs

- Event processing becomes eventually consistent.
- Operational complexity increases due to multiple running components.
- Message ordering is not guaranteed with the selected queue configuration.
- Additional monitoring is required for queue health.

---

## Alternatives Considered

### Direct Database Writes

Rejected.

Persisting directly from the API tightly couples request handling with storage and reduces resilience during downstream failures.

### Amazon EventBridge

Considered but not selected.

EventBridge offers rich event routing capabilities but introduces additional complexity beyond the requirements of the current platform. Amazon SQS provides a simpler and more cost-effective solution for point-to-point asynchronous processing.

### Apache Kafka

Rejected.

Kafka is well suited for high-throughput streaming platforms but introduces operational complexity unnecessary for the scale and objectives of this project.

---

## Implementation Summary

As of **Milestone 4 (v0.4.0)**:

- The API publishes canonical event envelopes to Amazon SQS.
- A dedicated Worker service performs asynchronous processing.
- Messages are removed from the queue only after successful processing.
- Failed processing leaves messages available for retry.
- CloudWatch metrics and structured logging provide operational visibility into the messaging pipeline.

---

## Related ADRs

- ADR-0002 – Infrastructure as Code with AWS CDK
- ADR-0006 – Canonical Event Envelope
- ADR-0007 – Dedicated Worker Service
- ADR-0008 – Amazon SQS Long Polling Strategy
- ADR-0009 – Event Processing Pipeline Architecture
- ADR-0010 – Observability Strategy

---

## Status

This decision remains valid as of **Milestone 4 (v0.4.0)** and continues to define the messaging architecture of the CloudOps Event Platform.