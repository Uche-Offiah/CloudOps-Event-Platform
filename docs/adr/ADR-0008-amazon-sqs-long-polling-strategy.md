# ADR-0008: Amazon SQS Long Polling Strategy

- **Status:** Accepted
- **Date:** 2026-07-28
- **Category:** Worker
- **Milestone:** 3
- **Decision Makers:** Project Owner

---

## Context

The dedicated Worker service (ADR-0007) continuously consumes messages from Amazon SQS.

A polling strategy was required to balance:

- Processing latency
- AWS API usage
- Infrastructure cost
- CPU utilization
- Operational efficiency

Amazon SQS supports both short polling and long polling.

Choosing the appropriate polling strategy directly affects the efficiency and responsiveness of the Worker service.

---

## Decision

The Worker will consume messages using **Amazon SQS Long Polling**.

Each receive request waits for messages to become available before returning, reducing unnecessary polling activity when the queue is idle.

The Worker continuously issues long-poll receive requests until the service is stopped.

---

## Architectural Principles

### Efficient Resource Utilization

The Worker minimizes unnecessary API calls while remaining responsive to newly available messages.

### Continuous Processing

Messages are consumed continuously without scheduled execution windows.

### Cost Optimization

Reducing empty receive requests lowers Amazon SQS API consumption and associated operating costs.

### Responsive Event Handling

Messages are processed shortly after arriving in the queue without excessive polling frequency.

---

## Rationale

Long polling provides several advantages over short polling.

It:

- Reduces empty responses.
- Lowers Amazon SQS API usage.
- Decreases CPU utilization during idle periods.
- Improves overall processing efficiency.
- Provides near real-time message consumption without constant polling.

These characteristics align with the operational goals of the CloudOps Event Platform while remaining simple to implement and maintain.

---

## Consequences

### Benefits

- Lower Amazon SQS request volume.
- Reduced infrastructure costs.
- Improved Worker efficiency.
- Lower CPU utilization during idle periods.
- Faster processing when messages arrive.

### Trade-offs

- Worker shutdown may be delayed briefly while a receive request completes.
- Long-lived receive requests require appropriate cancellation handling.
- Visibility timeout configuration becomes more important to avoid duplicate processing.

---

## Alternatives Considered

### Amazon SQS Short Polling

Rejected.

Short polling returns immediately when no messages are available, resulting in a significantly higher number of empty receive requests and unnecessary AWS API calls.

### Scheduled Queue Polling

Rejected.

Polling on a fixed schedule introduces unnecessary processing latency and prevents continuous event consumption.

### Event-Driven Invocation Using AWS Lambda

Considered but not selected.

Lambda provides automatic event source mapping but abstracts away the polling strategy and background service behavior. A dedicated Worker using long polling better supports the architectural and educational goals of this project.

---

## Implementation Summary

As of **Milestone 4 (v0.4.0)**:

The Worker:

- Uses Amazon SQS long polling.
- Waits up to **20 seconds** for available messages.
- Retrieves up to **five messages** per request.
- Processes messages sequentially within each polling cycle.
- Deletes messages only after successful processing.
- Uses cancellation tokens to support graceful shutdown.

This strategy has provided efficient message consumption while minimizing unnecessary Amazon SQS API requests.

---

## Related ADRs

- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0007 – Dedicated Worker Service
- ADR-0009 – Event Processing Pipeline Architecture
- ADR-0010 – Observability Strategy

---

## Status

This decision remains valid as of **Milestone 4 (v0.4.0)** and continues to define the message consumption strategy for the CloudOps Event Platform.