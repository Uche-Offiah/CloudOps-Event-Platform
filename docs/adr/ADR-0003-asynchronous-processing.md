# ADR-0003: Asynchronous Event Processing with Amazon SQS

* **Status:** Accepted
* **Date:** 2026-07-22
* **Decision Makers:** Project Owner

---

# Context

The CloudOps Event Platform receives operational events from external clients and internal services. These events may trigger multiple downstream operations, including persistence, notifications, auditing, and analytics.

A key architectural decision is whether the API should process requests synchronously by writing directly to the database, or whether it should decouple request handling from event processing using a message queue.

Because processing may grow in complexity over time and downstream services may experience temporary failures, the platform requires a design that prioritizes resiliency, scalability, and operational flexibility.

---

# Decision

The platform will use **Amazon Simple Queue Service (Amazon SQS)** as the primary asynchronous messaging mechanism between the API layer and background processing components.

The API will be responsible for:

* Validating incoming requests.
* Performing lightweight business validation.
* Publishing events to an SQS queue.
* Returning a response to the client as quickly as possible.

Background workers will be responsible for:

* Reading messages from the queue.
* Executing business workflows.
* Persisting events.
* Publishing notifications.
* Handling retries and failures.

Failed messages will be automatically redirected to a Dead Letter Queue (DLQ) after the configured maximum retry count.

---

# Rationale

## Separation of Responsibilities

The API should focus on accepting requests rather than performing long-running processing.

Using a queue separates request handling from background work, resulting in clearer service boundaries and improved maintainability.

---

## Improved User Experience

Clients receive responses immediately after the request has been validated and accepted.

Long-running operations no longer increase API response times.

---

## Resiliency

Temporary failures should not result in lost events.

If downstream services become unavailable, messages remain safely stored within Amazon SQS until they can be processed successfully.

---

## Independent Scaling

The API and background workers have different scaling characteristics.

Using a queue allows each component to scale independently based on demand.

For example:

* Increased API traffic can be absorbed by the queue.
* Additional workers can be added without modifying the API.
* Temporary spikes are smoothed rather than overwhelming downstream services.

---

## Retry Support

Amazon SQS provides built-in retry behavior through visibility timeouts.

If a worker fails while processing a message, the message becomes available again after the visibility timeout expires.

This reduces the need to implement custom retry logic.

---

## Failure Isolation

Messages that repeatedly fail processing should not block the remainder of the workload.

A Dead Letter Queue (DLQ) isolates problematic messages, allowing engineers to investigate and replay them without affecting normal processing.

---

## Future Extensibility

Using asynchronous messaging provides flexibility for future enhancements, including:

* Multiple worker services.
* Event replay.
* Fan-out processing.
* Scheduled processing.
* Event analytics.
* Audit pipelines.
* Integration with additional AWS services.

The messaging layer becomes the foundation for future platform capabilities without requiring changes to the API.

---

# Alternatives Considered

## Option 1: Direct Database Writes

### Description

The API writes events directly to DynamoDB during the request.

### Advantages

* Simple implementation.
* Fewer AWS resources.
* Lower initial complexity.

### Disadvantages

* Tighter coupling between the API and storage.
* Increased request latency.
* Database failures directly impact clients.
* Limited scalability.
* Difficult to introduce additional processing stages.

---

## Option 2: Amazon EventBridge

### Description

Publish operational events directly to Amazon EventBridge.

### Advantages

* Native event routing.
* Multiple subscribers.
* Rich filtering capabilities.
* Strong integration with AWS services.

### Disadvantages

* More complex routing model than required for the initial platform.
* Additional concepts for a foundational implementation.
* Does not provide the same work queue semantics as Amazon SQS.

### Decision

Amazon EventBridge remains a potential future enhancement but is intentionally deferred until the core event processing pipeline is established.

---

# Consequences

## Positive

* Improved resiliency.
* Better scalability.
* Reduced API latency.
* Clear separation of concerns.
* Built-in retry behavior.
* Operational visibility through queue metrics.
* Foundation for future event-driven capabilities.

## Trade-offs

* Increased architectural complexity.
* Eventual consistency between request acceptance and processing.
* Additional AWS resources to manage.
* Background workers become a required component of the platform.

---

# Operational Considerations

The platform should monitor:

* Queue depth.
* Message age.
* Dead Letter Queue size.
* Processing latency.
* Failed processing attempts.

CloudWatch alarms will be introduced in later milestones to notify operators when queue health exceeds acceptable thresholds.

---

# Future Improvements

Potential enhancements include:

* Event replay from the Dead Letter Queue.
* Multiple queues for workload isolation.
* FIFO queues where ordering guarantees are required.
* EventBridge integration for fan-out scenarios.
* Lambda event source mappings.
* Auto Scaling based on queue depth.
* Distributed tracing across asynchronous boundaries.

---

# Decision Summary

The platform adopts Amazon SQS as the primary messaging backbone because it provides a resilient, scalable, and operationally mature mechanism for asynchronous event processing.

This decision establishes a strong architectural foundation while keeping the initial implementation focused and maintainable. More advanced event-routing patterns may be introduced in future iterations as the platform evolves.
