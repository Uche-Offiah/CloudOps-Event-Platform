# ADR-0007: Dedicated Worker Service

- **Status:** Accepted
- **Date:** 2026-07-27
- **Category:** Worker
- **Milestone:** 3
- **Decision Makers:** Project Owner

---

## Context

Following the adoption of asynchronous messaging (ADR-0003), the platform required a component responsible for consuming messages from Amazon SQS and executing downstream processing.

An architectural decision was needed regarding where message processing should occur.

Possible approaches included:

- Processing messages within the API application.
- Running scheduled jobs.
- Deploying a dedicated background service.

The solution needed to:

- Process messages continuously.
- Operate independently of HTTP traffic.
- Scale separately from the API.
- Remain resilient during temporary failures.
- Support future processing capabilities without affecting request handling.

---

## Decision

The platform will implement message consumption as a **dedicated Worker service**.

The Worker is deployed as an independent .NET application responsible for:

- Polling Amazon SQS.
- Processing event messages.
- Persisting canonical event envelopes.
- Publishing operational metrics.
- Producing structured logs.

The Worker has no HTTP endpoints and communicates exclusively through asynchronous messaging.

---

## Architectural Principles

### Single Responsibility

The Worker is responsible only for background event processing.

HTTP request handling remains the responsibility of the API.

### Independent Deployment

The Worker may be deployed, restarted, upgraded, or scaled independently from the API.

### Asynchronous Processing

Message consumption occurs continuously without blocking request processing.

### Fault Isolation

Failures in the Worker do not prevent the API from accepting new events.

Likewise, temporary API outages do not interrupt processing of messages already present in the queue.

### Infrastructure Independence

Business processing is coordinated through the Application layer.

AWS-specific integrations remain confined to the Infrastructure layer.

---

## Rationale

Separating background processing from the API provides clear operational boundaries.

Benefits include:

- Independent scaling.
- Better resource utilization.
- Simpler operational troubleshooting.
- Improved resilience.
- Cleaner architectural separation.

This approach also mirrors how event-driven workloads are commonly implemented in production cloud environments.

---

## Consequences

### Benefits

- API responsiveness remains unaffected by processing workloads.
- Worker throughput can be scaled independently.
- Improved fault isolation.
- Easier monitoring and troubleshooting.
- Supports future parallel processing strategies.

### Trade-offs

- Additional deployable service.
- Separate logging and monitoring requirements.
- Increased deployment complexity.
- Operational coordination between API and Worker.

---

## Alternatives Considered

### BackgroundService Hosted Inside the API

Rejected.

Although technically simpler, coupling HTTP request handling and continuous message processing within the same process reduces deployment flexibility and complicates independent scaling.

### Scheduled Polling Jobs

Rejected.

Periodic jobs introduce unnecessary processing latency and do not provide the continuous consumption required for an event-driven platform.

### AWS Lambda Event Source Mapping

Considered but not selected.

Lambda provides automatic scaling and event-driven execution but abstracts away long-running worker behavior. A dedicated Worker better supports the educational goals of demonstrating service architecture, dependency injection, structured logging, and operational monitoring.

---

## Implementation Summary

As of **Milestone 4 (v0.4.0)**:

The Worker service:

- Continuously polls Amazon SQS using long polling.
- Creates a scoped dependency injection container for each polling cycle.
- Delegates business processing to the Application layer.
- Persists canonical event envelopes to Amazon DynamoDB.
- Deletes messages only after successful processing.
- Emits CloudWatch metrics.
- Produces structured logs using correlation scopes.

The Worker remains completely independent from the HTTP API while sharing the same Domain, Application, and Infrastructure layers.

---

## Related ADRs

- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0004 – Clean Architecture and Vertical Slice Organization
- ADR-0006 – Canonical Event Envelope
- ADR-0008 – Amazon SQS Long Polling Strategy
- ADR-0009 – Event Processing Pipeline Architecture
- ADR-0010 – Observability Strategy

---

## Status

This decision remains valid as of **Milestone 4 (v0.4.0)** and continues to define the background processing architecture of the CloudOps Event Platform.