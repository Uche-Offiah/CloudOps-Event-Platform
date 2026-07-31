# Milestone 4 – Observability and Operational Readiness

**Milestone:** 4  
**Release:** v0.4.0  
**Date Completed:** July 2026

---

# Objective

The objective of this milestone was to make the CloudOps Event Platform operationally observable and production-ready.

Previous milestones established event ingestion, asynchronous processing, and persistence. This milestone focused on enabling engineers to monitor, troubleshoot, and verify the health of the platform through structured logging, custom metrics, dashboards, alarms, and health checks.

The goal was to ensure operational issues could be detected and diagnosed quickly without relying solely on application logs.

---

# What Was Built

The following capabilities were introduced:

- Structured logging improvements
- Correlation ID propagation
- CloudWatch custom metrics
- CloudWatch dashboard
- CloudWatch alarms
- Amazon SNS alert notifications
- API health endpoint
- Operational validation runbooks
- End-to-end observability verification

At the conclusion of this milestone, the platform provided both functional correctness and operational visibility.

---

# Key Architectural Decisions

Several important operational decisions were introduced.

### Observability by Design

Observability became a first-class architectural concern rather than an operational afterthought.

Logs, metrics, dashboards, alarms, and health checks were designed together as complementary sources of operational telemetry.

Reference:

- ADR-0010 – Observability Strategy

---

### Correlation Across Services

Every event receives a CorrelationId during ingestion.

That identifier is propagated through:

- API
- Amazon SQS
- Worker
- DynamoDB persistence
- Structured logs

This allows engineers to reconstruct the complete lifecycle of a single event.

---

### Metrics Over Log Searching

Operational dashboards should answer common operational questions without requiring engineers to manually inspect logs.

CloudWatch custom metrics provide aggregated visibility into:

- Accepted events
- Successfully processed events
- Duplicate events
- Failed processing attempts

---

### Operational Health Verification

A lightweight health endpoint was introduced to verify API availability.

This provides a simple readiness signal for operators and future deployment automation.

---

# Technical Challenges

Several implementation challenges emerged.

## Cross-Service Correlation

Maintaining consistent contextual information across independently executing services required careful propagation of correlation identifiers.

### Resolution

Structured logging scopes were introduced to ensure EventId, CorrelationId, EventType, and Source remained available throughout the processing pipeline.

---

## Operational Metrics

Application logs describe individual events but do not communicate platform health.

### Resolution

Custom CloudWatch metrics were published for key operational events, enabling dashboards and alarms to provide aggregate visibility.

---

## Infrastructure Monitoring

Operational dashboards and alarms required CloudFormation resources rather than application code alone.

### Resolution

CloudWatch dashboards, alarms, and Amazon SNS notifications were provisioned using reusable AWS CDK constructs.

---

# Solutions Implemented

The following implementation patterns were adopted.

- Structured logging scopes.
- Correlation identifier propagation.
- CloudWatch custom metrics.
- CloudWatch dashboards.
- CloudWatch alarms.
- Amazon SNS notifications.
- Health endpoint.
- Operational validation procedures.
- Infrastructure-as-Code for monitoring resources.

---

# Lessons Learned

Several operational engineering lessons emerged.

## Logs answer "What happened?"

Structured logs provide detailed information about individual processing operations.

---

## Metrics answer "How healthy is the platform?"

Metrics reveal operational trends that individual log entries cannot.

---

## Dashboards reduce investigation time

Well-designed dashboards allow engineers to quickly determine whether issues are isolated or systemic.

---

## Alarms should be actionable

An alarm is valuable only when it indicates a condition requiring operator attention.

Avoid excessive alarms that contribute to alert fatigue.

---

## Correlation is essential in distributed systems

Without correlation identifiers, reconstructing an event's journey across multiple services becomes significantly more difficult.

---

# Best Practices Identified

- Use structured logging instead of free-form text.
- Propagate correlation identifiers across service boundaries.
- Measure operational health through metrics.
- Build dashboards that answer common operational questions.
- Configure actionable alarms.
- Treat observability as part of the architecture rather than an operational add-on.
- Manage monitoring infrastructure through AWS CDK.

---

# Future Improvements

The following enhancements were intentionally deferred.

- Distributed tracing with AWS X-Ray or OpenTelemetry.
- Log retention policies.
- Alarm escalation strategies.
- Additional business metrics.
- Service Level Indicators (SLIs).
- Service Level Objectives (SLOs).
- Centralized operational reporting.
- Dead Letter Queue monitoring.

These enhancements can build upon the observability foundation established during this milestone.

---

# Related ADRs

- ADR-0002 – Infrastructure as Code with AWS CDK
- ADR-0005 – Event Ingestion API Design
- ADR-0007 – Dedicated Worker Service
- ADR-0009 – Event Processing Pipeline Architecture
- ADR-0010 – Observability Strategy

---

# Related Runbooks

- Observability Validation
- Testing Worker SQS Connectivity
- Deploying the Platform

---

# Milestone Outcome

Milestone 4 completed the transition of the CloudOps Event Platform from a functioning distributed application to an operationally observable platform.

The platform now provides structured logging, end-to-end correlation, custom CloudWatch metrics, dashboards, alarms, health checks, and operational runbooks. These capabilities enable engineers to monitor system health, investigate failures, and verify deployments using production-oriented operational practices.