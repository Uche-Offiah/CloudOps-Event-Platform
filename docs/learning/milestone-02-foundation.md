# Milestone 2 – Event Ingestion Foundation

**Milestone:** 2  
**Release:** v0.2.0  
**Date Completed:** July 2026

---

# Objective

The objective of this milestone was to establish the foundation of the CloudOps Event Platform by implementing the first end-to-end event ingestion capability.

This milestone transformed the project from an infrastructure-only solution into a functioning cloud application capable of accepting events through an HTTP API and publishing them asynchronously for downstream processing.

The implementation focused on creating a clean architectural boundary between request handling and event processing while establishing patterns that would support future milestones.

---

# What Was Built

The following capabilities were introduced:

- ASP.NET Core Minimal API
- Event submission endpoint
- Request validation
- Canonical Event Envelope
- Correlation ID generation
- Event publishing abstraction
- Amazon SQS integration
- Dependency Injection configuration
- Clean Architecture project boundaries
- Vertical Slice feature organization

At the conclusion of this milestone, clients could successfully submit events into the CloudOps platform for asynchronous processing.

---

# Key Architectural Decisions

This milestone introduced several architectural decisions that continue to guide the platform.

### Event-Driven Communication

Rather than processing events synchronously, the API accepts requests and publishes them to Amazon SQS for downstream processing.

This improves scalability, resilience, and response time.

Reference:

- ADR-0003 – Asynchronous Event Processing with Amazon SQS

---

### Clean Architecture

Business logic remains isolated from infrastructure implementations.

AWS SDK integrations are confined to the Infrastructure project while the Application layer communicates through interfaces.

Reference:

- ADR-0004 – Clean Architecture and Vertical Slice Organization

---

### Canonical Event Envelope

Every accepted request is transformed into a consistent event structure before publication.

This ensures all downstream components process the same message format.

Reference:

- ADR-0006 – Canonical Event Envelope

---

# Technical Challenges

Several implementation challenges were encountered.

## Separating Business Logic from Infrastructure

The initial implementation risked coupling request handling directly to AWS SDK calls.

This was resolved by introducing publisher abstractions and dependency injection.

---

## Designing the Event Contract

Determining which information belonged in platform metadata versus business payload required careful consideration.

Separating these concerns resulted in a cleaner and more extensible event model.

---

## Feature Organization

Rather than using traditional Controller and Service folders, the project adopted Vertical Slice Architecture.

Although unfamiliar initially, this organization scales significantly better as features grow.

---

# Solutions Implemented

The following implementation patterns were adopted.

- Minimal APIs for lightweight HTTP endpoints.
- Dependency Injection throughout the application.
- Application interfaces separating business logic from AWS implementations.
- Immutable Event Envelope records.
- JSON payload preservation using `JsonElement`.
- Correlation ID generation during request acceptance.

---

# Lessons Learned

Several important engineering lessons emerged during this milestone.

## Thin APIs are easier to maintain

Endpoints should coordinate requests rather than implement business logic.

---

## Contracts deserve careful design

The Event Envelope became the foundation for every later milestone.

A well-designed contract significantly reduced future implementation effort.

---

## Architecture should come before features

Investing time in architectural boundaries simplified subsequent milestones involving persistence, background processing, and observability.

---

# Best Practices Identified

- Keep endpoints focused on orchestration.
- Use dependency injection consistently.
- Publish immutable events.
- Separate platform metadata from business payload.
- Prefer asynchronous communication for long-running workflows.
- Maintain infrastructure independence through abstractions.

---

# Future Improvements

The following enhancements were intentionally deferred.

- Dedicated Worker service
- Event persistence
- Duplicate detection
- Dead Letter Queue
- Custom CloudWatch metrics
- Operational dashboards
- Health endpoints

These capabilities were implemented in later milestones.

---

# Related ADRs

- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0004 – Clean Architecture and Vertical Slice Organization
- ADR-0005 – Event Ingestion API Design
- ADR-0006 – Canonical Event Envelope

---

# Related Runbooks

- Testing Worker SQS Connectivity
- Observability Validation

---

# Milestone Outcome

Milestone 2 established the application foundation for the CloudOps Event Platform.

The platform now supported asynchronous event ingestion while maintaining clean architectural boundaries, providing the basis for the background processing, persistence, and observability capabilities implemented in subsequent milestones.