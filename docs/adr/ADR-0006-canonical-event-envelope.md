# ADR-0006: Canonical Event Envelope

- **Status:** Accepted
- **Date:** 2026-07-25
- **Category:** Messaging
- **Milestone:** 2
- **Decision Makers:** Project Owner

---

## Context

The CloudOps Event Platform receives events from external producers and processes them asynchronously through multiple components.

Without a standardized event structure, each producer or consumer could introduce different payload formats, making interoperability increasingly difficult as the platform evolves.

The platform requires a single, immutable event contract that:

- Represents every operational event consistently.
- Supports asynchronous processing.
- Enables end-to-end traceability.
- Allows payloads to evolve independently of transport metadata.
- Remains stable as additional consumers are introduced.

---

## Decision

All events exchanged within the platform shall be represented using a **canonical event envelope**.

The envelope contains platform metadata required for routing, persistence, observability, and correlation, while preserving the business payload as an opaque JSON document.

The canonical envelope consists of:

| Field | Purpose |
|--------|---------|
| EventId | Globally unique identifier for the event |
| EventType | Business event classification |
| Source | Originating system or producer |
| OccurredAtUtc | Timestamp representing event acceptance |
| CorrelationId | Correlates activity across distributed components |
| Version | Envelope schema version |
| Payload | Business-specific event data |

The payload is intentionally independent of the surrounding metadata.

---

## Architectural Principles

### Canonical Representation

Every internal component communicates using the same event structure.

### Immutable Events

Once created, an event envelope is never modified.

Any subsequent business change is represented by publishing a new event.

### Separation of Metadata and Payload

Operational metadata is managed by the platform.

Business data remains contained within the payload.

### Versioned Contract

The envelope supports explicit versioning to allow future evolution without breaking compatibility.

### Technology Independence

The envelope represents the platform contract rather than any specific transport or persistence model.

---

## Rationale

A canonical event envelope provides a common language across the platform.

It simplifies:

- Event routing
- Persistence
- Logging
- Metrics
- Correlation
- Future integrations

The separation between metadata and payload allows operational capabilities to evolve independently from business event schemas.

---

## Consequences

### Benefits

- Consistent event representation.
- Simplified persistence.
- Easier observability.
- Stable internal contract.
- Improved interoperability between producers and consumers.
- Supports future replay and auditing scenarios.

### Trade-offs

- Additional metadata slightly increases message size.
- Envelope versioning must be maintained carefully.
- Consumers must distinguish between platform metadata and business payload.

---

## Alternatives Considered

### Event-Type Specific Message Structures

Rejected.

Allowing each event type to define its own transport structure would increase coupling and complicate routing, persistence, and observability.

### CloudEvents Specification

Considered but not selected.

CloudEvents provides a standardized event format for interoperability between platforms. For the educational objectives and scope of this project, a simplified canonical envelope provides the required capabilities while remaining easier to understand and evolve.

### Raw JSON Payloads

Rejected.

Using payloads without standardized metadata would make correlation, auditing, and operational monitoring significantly more difficult.

---

## Implementation Summary

As of **Milestone 4 (v0.4.0)**:

- Every accepted request is transformed into a canonical event envelope.
- Event identifiers and correlation identifiers are generated during ingestion.
- The envelope is published unchanged to Amazon SQS.
- The Worker processes and persists the complete envelope.
- Structured logging and CloudWatch metrics use the envelope metadata to provide end-to-end observability.

---

## Related ADRs

- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0005 – Event Ingestion API Design
- ADR-0007 – Dedicated Worker Service
- ADR-0009 – Event Processing Pipeline Architecture
- ADR-0010 – Observability Strategy

---

## Status

This decision remains valid as of **Milestone 4 (v0.4.0)** and continues to define the canonical event contract for the CloudOps Event Platform.