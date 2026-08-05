# ADR-0010: Observability Strategy

- **Status:** Accepted
- **Date:** 2026-07-31
- **Category:** Operations
- **Milestone:** 4
- **Decision Makers:** Project Owner

---

## Context

As the CloudOps Event Platform evolved into a distributed, event-driven system, understanding system behavior became increasingly important.

The platform now consists of multiple independently executing components including:

- HTTP API
- Background Worker
- Amazon SQS
- Amazon DynamoDB
- Amazon CloudWatch
- Amazon SNS

Failures may occur at different stages of the processing pipeline and cannot always be diagnosed through application logs alone.

The platform therefore requires an observability strategy that enables engineers to:

- Understand system health.
- Detect failures quickly.
- Correlate activity across services.
- Measure operational performance.
- Respond to production incidents.

---

## Decision

Observability shall be treated as a core architectural capability.

The platform will provide operational visibility through multiple complementary mechanisms:

- Structured logging
- Correlation identifiers
- CloudWatch custom metrics
- CloudWatch dashboards
- CloudWatch alarms
- Health check endpoints

Each component contributes operational telemetry using consistent conventions.

---

## Architectural Principles

### Structured Logging

Application logs are emitted using structured properties rather than free-form text.

Important identifiers—including EventId, CorrelationId, EventType, and Source—are included within logging scopes to support querying and analysis.

### End-to-End Correlation

Every accepted event receives a CorrelationId during ingestion.

The same CorrelationId is propagated throughout the complete processing lifecycle, enabling engineers to reconstruct an event's journey across the platform.

### Metrics-Driven Operations

Operational health is measured through custom CloudWatch metrics rather than relying exclusively on log inspection.

Metrics provide aggregated visibility into platform behavior over time.

### Proactive Monitoring

CloudWatch alarms notify operators when predefined operational thresholds are exceeded.

The platform emphasizes early detection of abnormal behavior.

### Health Verification

Health endpoints provide a lightweight mechanism for verifying service availability and readiness.

They are intended for operational monitoring rather than detailed diagnostics.

---

## Rationale

Modern distributed systems require more than application logs.

Combining logs, metrics, dashboards, alarms, and health checks provides multiple perspectives on system behavior.

This approach:

- Reduces mean time to detection (MTTD).
- Improves troubleshooting.
- Supports operational decision making.
- Enables production-style monitoring practices.
- Aligns with cloud-native operational principles.

---

## Consequences

### Benefits

- Improved operational visibility.
- Faster root-cause analysis.
- End-to-end event correlation.
- Reduced reliance on manual log inspection.
- Production-ready monitoring capabilities.
- Easier verification of platform health.

### Trade-offs

- Additional infrastructure resources.
- Increased implementation effort.
- Operational telemetry must be maintained alongside application code.
- Dashboard and alarm thresholds require periodic review as workloads evolve.

---

## Alternatives Considered

### Log-Only Monitoring

Rejected.

Application logs alone provide insufficient visibility into overall system behavior and make proactive detection difficult.

### Third-Party Observability Platforms

Considered but not selected.

Platforms such as Datadog, New Relic, and Grafana provide advanced observability capabilities. For this project, native AWS services provide sufficient functionality while keeping the architecture simple and minimizing external dependencies.

### Metrics Without Correlation

Rejected.

Metrics identify trends but cannot explain the lifecycle of individual events. Correlation identifiers complement aggregated metrics by enabling detailed event tracing through logs.

---

## Implementation Summary

As of **Milestone 4 (v0.4.0)**, the platform provides:

### Structured Logging

- Correlation scopes across API and Worker.
- Structured properties for EventId, CorrelationId, EventType, and Source.
- Consistent logging across application layers.

### CloudWatch Custom Metrics

- EventsAccepted
- EventsProcessed
- DuplicateEvents
- EventsFailed

### CloudWatch Dashboard

Operational dashboards display:

- Successful event processing.
- Duplicate event detection.
- Processing failures.
- Alarm status.

### CloudWatch Alarms

Operational alarms monitor:

- Failed event processing.
- Duplicate event activity.

Alarm notifications are published through Amazon SNS.

### Health Endpoint

The API exposes a lightweight health endpoint for service availability verification.

---

## Related ADRs

- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0005 – Event Ingestion API Design
- ADR-0007 – Dedicated Worker Service
- ADR-0009 – Event Processing Pipeline Architecture

---

## Status

This decision remains valid as of **Milestone 4 (v0.4.0)** and defines the operational observability strategy for the CloudOps Event Platform.