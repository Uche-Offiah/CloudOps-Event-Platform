# Milestone 3 – Asynchronous Event Processing

**Milestone:** 3  
**Release:** v0.3.0  
**Date Completed:** July 2026

---

# Objective

The objective of this milestone was to complete the event processing pipeline by introducing a dedicated background Worker capable of consuming events from Amazon SQS, persisting them to Amazon DynamoDB, and supporting reliable, idempotent processing.

This milestone transformed the platform from an event producer into a complete event-driven processing system.

---

# What Was Built

The following capabilities were introduced:

- Dedicated Worker service
- Amazon SQS long polling
- Event deserialization
- Event processing pipeline
- Amazon DynamoDB persistence
- Idempotent event storage
- Duplicate event detection
- Scoped dependency injection within the Worker
- Processing result abstraction
- Improved structured logging

At the conclusion of this milestone, events flowed successfully through the complete processing lifecycle from API submission to durable storage.

---

# Key Architectural Decisions

Several significant architectural decisions were introduced during this milestone.

### Dedicated Worker Service

Background processing was separated from the HTTP API into an independent Worker service.

This separation improves scalability, resilience, deployment flexibility, and operational isolation.

Reference:

- ADR-0007 – Dedicated Worker Service

---

### Long Polling

The Worker uses Amazon SQS long polling to reduce unnecessary API requests while maintaining responsive event processing.

Reference:

- ADR-0008 – Amazon SQS Long Polling Strategy

---

### Event Processing Pipeline

Rather than combining deserialization, validation, persistence, and infrastructure operations into one method, the platform adopted a staged processing pipeline.

Each stage performs a single responsibility.

Reference:

- ADR-0009 – Event Processing Pipeline Architecture

---

### Idempotent Persistence

Duplicate events are expected in distributed systems.

Amazon DynamoDB conditional writes were adopted to ensure an event can only be persisted once.

Duplicate writes are treated as operational events rather than application failures.

---

# Technical Challenges

Several implementation challenges were encountered during this milestone.

## Scoped Dependencies Inside Background Services

Background services are registered as singletons, while application services such as repositories are scoped.

Attempting to inject scoped services directly into the Worker resulted in dependency injection validation failures.

### Resolution

A new dependency injection scope is created during each polling cycle using `IServiceScopeFactory`.

---

## Duplicate Event Handling

Network retries and distributed messaging introduce the possibility of duplicate message delivery.

Initially, duplicate writes were treated as errors.

### Resolution

DynamoDB conditional writes (`attribute_not_exists(EventId)`) were introduced to enforce idempotency.

Duplicate events are detected through `ConditionalCheckFailedException` and recorded without corrupting stored data.

---

## Worker Lifetime Management

Gracefully stopping the Worker while long polling required careful cancellation handling.

### Resolution

The Worker propagates cancellation tokens throughout the processing pipeline and exits cleanly when shutdown is requested.

---

# Solutions Implemented

The following implementation patterns were adopted.

- Dedicated Worker process.
- Amazon SQS long polling.
- Scoped dependency injection.
- Processing result abstraction.
- Repository abstraction.
- DynamoDB conditional writes.
- Structured processing logs.
- Explicit success and failure handling.
- Message deletion only after successful processing.

---

# Lessons Learned

This milestone introduced several important distributed systems concepts.

## Background services require different dependency management

Hosted services should not directly depend on scoped services.

Creating explicit scopes avoids lifetime mismatches.

---

## Distributed systems must expect duplicates

Duplicate message delivery is not exceptional—it is an expected characteristic of distributed messaging systems.

Applications should be designed to process duplicate messages safely.

---

## Reliable processing depends on message acknowledgement

Messages should only be deleted from the queue after successful processing.

Deleting messages prematurely risks permanent data loss.

---

## Small processing stages improve maintainability

Separating deserialization, validation, persistence, and logging significantly simplifies debugging and future enhancements.

---

# Best Practices Identified

- Separate request handling from background processing.
- Keep Worker responsibilities narrowly focused.
- Use long polling to reduce unnecessary queue requests.
- Design persistence to be idempotent.
- Delete queue messages only after successful processing.
- Treat duplicate delivery as normal system behavior.
- Use dependency injection scopes correctly inside hosted services.

---

# Future Improvements

The following enhancements were intentionally deferred.

- Dead Letter Queue (DLQ)
- Retry backoff strategies
- Batch persistence
- Parallel message processing
- Event replay capabilities
- Message enrichment pipeline
- Multiple event consumers

These enhancements can be added without changing the overall architecture established during this milestone.

---

# Related ADRs

- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0007 – Dedicated Worker Service
- ADR-0008 – Amazon SQS Long Polling Strategy
- ADR-0009 – Event Processing Pipeline Architecture

---

# Related Runbooks

- Testing Worker SQS Connectivity
- Duplicate Event Verification
- Observability Validation

---

# Milestone Outcome

Milestone 3 completed the core event processing architecture of the CloudOps Event Platform.

The platform now supports reliable asynchronous processing through a dedicated Worker, durable persistence in Amazon DynamoDB, idempotent event handling, and a well-defined processing pipeline. These capabilities established a production-oriented foundation for the operational observability enhancements introduced in Milestone 4.