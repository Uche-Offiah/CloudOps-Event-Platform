# ADR-0005: Event Ingestion API Design

- **Status:** Accepted
- **Date:** 2026-07-24
- **Category:** API
- **Milestone:** 2
- **Decision Makers:** Project Owner

---

## Context

The CloudOps Event Platform requires an external interface through which clients can submit operational events for processing.

The API should:

- Accept events from external producers.
- Validate incoming requests.
- Return a response immediately.
- Avoid performing long-running processing synchronously.
- Remain stable as downstream processing evolves.

The interface should expose a consistent contract while allowing the internal processing pipeline to change independently.

---

## Decision

The platform exposes a RESTful HTTP endpoint for event submission.

The API is responsible only for:

- Receiving requests.
- Validating input.
- Constructing the canonical event envelope.
- Assigning identifiers and timestamps.
- Publishing the event asynchronously.

The API deliberately does **not** perform persistence or business processing.

Successful submission indicates that the event has been accepted for asynchronous processing rather than fully processed.

---

## Architectural Principles

### Thin API Layer

HTTP endpoints coordinate requests but do not contain business logic.

Application behavior is delegated to the Application layer.

### Asynchronous Acceptance

Clients receive confirmation that the platform accepted the event.

Subsequent processing occurs independently through the messaging pipeline.

### Stable Public Contract

External clients interact with a stable HTTP interface regardless of internal implementation changes.

### Canonical Event Creation

Every accepted request is transformed into the platform's canonical event envelope before publication.

### Transport Independence

Business rules remain independent of HTTP-specific concerns.

---

## Rationale

Separating request acceptance from processing provides:

- Faster client responses.
- Better scalability.
- Improved resilience.
- Loose coupling between producers and consumers.

The API acts as the entry point into the event-driven platform rather than as the processing engine.

---

## Consequences

### Benefits

- Low request latency.
- Improved scalability.
- Clear separation between transport and business logic.
- Easier evolution of downstream processing.
- Consistent public interface.

### Trade-offs

- Clients cannot determine processing completion from the HTTP response alone.
- Event processing becomes eventually consistent.
- Additional operational monitoring is required to observe downstream processing.

---

## Alternatives Considered

### Synchronous Processing

Rejected.

Executing persistence and processing within the request pipeline would increase response times, tightly couple the API to downstream components, and reduce resilience.

### GraphQL

Rejected.

The platform exposes a single command-oriented operation rather than a flexible query interface, making REST a simpler and more appropriate choice.

### gRPC

Considered but not selected.

Although gRPC offers strong performance characteristics, a REST interface provides broader interoperability for external producers and simpler testing during development.

---

## Implementation Summary

As of **Milestone 4 (v0.4.0)**:

- Event ingestion is implemented using ASP.NET Core Minimal APIs.
- Input validation occurs before publication.
- Event identifiers and correlation identifiers are generated during request handling.
- Events are published asynchronously to Amazon SQS.
- Successful requests return **HTTP 202 Accepted** with the generated EventId and acceptance timestamp.
- Structured logging captures request context for end-to-end correlation.

---

## Related ADRs

- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0004 – Clean Architecture and Vertical Slice Organization
- ADR-0006 – Canonical Event Envelope
- ADR-0009 – Event Processing Pipeline Architecture
- ADR-0010 – Observability Strategy

---

## Status

This decision remains valid as of **Milestone 4 (v0.4.0)** and continues to define the external event ingestion interface of the CloudOps Event Platform.