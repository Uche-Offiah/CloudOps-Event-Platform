# Observability Architecture

**Applies To:** CloudOps Event Platform  
**Release:** v0.4.0

---

# Overview

The CloudOps Event Platform is designed with observability as a core architectural capability rather than an operational afterthought.

The platform provides visibility into application behavior through structured logging, custom CloudWatch metrics, dashboards, alarms, health endpoints, and correlation identifiers. Together, these capabilities enable engineers to monitor system health, investigate failures, and understand the lifecycle of individual events across distributed services.

Observability supports both day-to-day operations and incident response while providing the foundation for future enhancements such as distributed tracing and Service Level Objectives (SLOs).

---

# Objectives

The observability architecture is designed to achieve the following objectives:

- Provide end-to-end visibility across the event processing pipeline.
- Enable rapid troubleshooting through structured logs.
- Measure operational health using custom metrics.
- Surface trends through CloudWatch dashboards.
- Detect abnormal conditions using CloudWatch alarms.
- Notify operators through Amazon SNS.
- Correlate activity across independently running services.
- Verify application availability through health checks.

---

# Observability Pillars

The platform implements five complementary observability capabilities.

## 1. Structured Logging

Structured logging captures operational events using consistent log templates and contextual properties.

Every significant processing stage emits structured log entries.

Examples include:

- Event submission
- Message reception
- Event processing
- DynamoDB persistence
- Duplicate detection
- Queue acknowledgement
- Processing failures

Logs include contextual metadata that allows events to be filtered and searched efficiently.

Typical log properties include:

- EventId
- CorrelationId
- EventType
- Source

---

## 2. Metrics

Logs describe individual events, while metrics describe platform health.

Custom CloudWatch metrics are published to summarize key operational behaviors.

Namespace:

```
CloudOps/EventPlatform
```

### EventsAccepted

Published when the API successfully accepts an event.

Purpose:

Measures ingestion rate.

---

### EventsProcessed

Published after successful Worker processing and persistence.

Purpose:

Measures successful throughput.

---

### DuplicateEvents

Published whenever duplicate event processing is detected.

Purpose:

Measures idempotency behavior.

---

### EventsFailed

Published whenever processing cannot be completed successfully.

Purpose:

Measures operational failures requiring investigation.

---

# Dashboard

A CloudWatch Dashboard aggregates the platform's operational metrics into a single view.

The dashboard provides visibility into:

- Accepted events
- Successfully processed events
- Duplicate events
- Failed events
- Alarm status

The dashboard serves as the primary operational overview for the platform.

---

# Alarm Strategy

CloudWatch Alarms monitor metrics that indicate operational issues.

Current alarms include:

## DuplicateEvents Alarm

Purpose:

Detect unexpected increases in duplicate event processing.

Normal duplicate processing is expected in distributed systems; however, sustained increases may indicate upstream retry behavior or infrastructure issues.

---

## EventsFailed Alarm

Purpose:

Detect failures occurring during Worker processing.

This alarm provides early notification of processing failures before they impact downstream systems.

---

## Notifications

Alarm state changes publish notifications to an Amazon SNS Topic.

Subscribers may include:

- Email
- Incident management systems
- Chat integrations
- Future automation workflows

---

# Health Monitoring

The API exposes a lightweight health endpoint.

```
GET /health
```

Expected response:

```
HTTP 200 OK
```

The health endpoint provides a simple readiness signal and can be used by deployment automation, load balancers, and monitoring systems.

Future implementations may expand health checks to include dependencies such as Amazon SQS and DynamoDB.

---

# Correlation Flow

Every accepted request receives a CorrelationId during API processing.

The CorrelationId is propagated throughout the platform to enable end-to-end tracing of a single event.

```
Client
   │
   ▼
API
   │
   ▼
Amazon SQS
   │
   ▼
Worker
   │
   ▼
DynamoDB
   │
   ▼
CloudWatch Logs
```

This allows engineers to reconstruct the lifecycle of an event using a single identifier.

---

# Event Processing Observability

The following sequence illustrates the telemetry generated during normal processing.

```
Client
   │
   ▼
Submit Event
   │
   │ Log
   │ Metric: EventsAccepted
   ▼
Amazon SQS
   │
   ▼
Worker Receives Message
   │
   │ Log
   ▼
Persist Event
   │
   │ Log
   │ Metric: EventsProcessed
   ▼
Delete Queue Message
   │
   │ Log
   ▼
Processing Complete
```

If duplicate processing occurs:

```
Worker
   │
   ▼
Conditional Write Failure
   │
   │ Log
   │ Metric: DuplicateEvents
   ▼
Continue Processing
```

If processing fails:

```
Worker
   │
   ▼
Exception
   │
   │ Log
   │ Metric: EventsFailed
   ▼
CloudWatch Alarm
   │
   ▼
Amazon SNS Notification
```

---

# Operational Workflow

Normal operational monitoring follows this sequence:

1. Review the CloudWatch Dashboard.
2. Investigate alarms, if present.
3. Identify affected EventId or CorrelationId.
4. Search structured logs.
5. Trace the event through API and Worker processing.
6. Verify persistence in DynamoDB.
7. Confirm metric behavior.
8. Resolve the underlying issue.

---

# Design Principles

The observability architecture follows several guiding principles.

## Structured First

Operational data should be machine-readable rather than embedded in free-form log messages.

---

## Correlation Everywhere

Every significant operation should include sufficient context to reconstruct an event's lifecycle.

---

## Metrics Complement Logs

Metrics summarize platform health while logs explain individual events.

Neither replaces the other.

---

## Infrastructure as Code

Dashboards, alarms, and notification infrastructure are provisioned through AWS CDK to ensure consistency across environments.

---

## Actionable Alerts

Every alarm should indicate a condition requiring operator attention.

Avoid alarms that generate unnecessary operational noise.

---

# Future Enhancements

Potential improvements include:

- AWS X-Ray integration
- OpenTelemetry support
- Distributed tracing
- Service Level Indicators (SLIs)
- Service Level Objectives (SLOs)
- Error budget tracking
- Log retention policies
- Dashboard automation
- Business-level operational metrics
- Dead Letter Queue monitoring

These enhancements can be introduced without changing the current architectural model.

---

# Related ADRs

- ADR-0002 – Infrastructure as Code with AWS CDK
- ADR-0005 – Event Ingestion API Design
- ADR-0007 – Dedicated Worker Service
- ADR-0009 – Event Processing Pipeline Architecture
- ADR-0010 – Observability Strategy

---

# Related Learning Notes

- Milestone 3 – Asynchronous Event Processing
- Milestone 4 – Observability and Operational Readiness

---

# Related Runbooks

- Deploying the CloudOps Event Platform
- Testing Worker SQS Connectivity
- Duplicate Event Verification
- Observability Validation