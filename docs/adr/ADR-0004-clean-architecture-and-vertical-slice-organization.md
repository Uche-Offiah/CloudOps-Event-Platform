# ADR-0004: Clean Architecture and Vertical Slice Organization

* **Status:** Accepted
* **Category:** Architecture
* **Date:** 2026-07-22
* **Decision Makers:** Project Owner

---

# Context

The CloudOps Event Platform requires an application architecture that can support future growth while maintaining clear separation between business logic, infrastructure concerns, and external interfaces.

As the platform evolves, new capabilities will be introduced, including:

* Event ingestion.
* Background processing.
* Notifications.
* Persistence.
* Authentication.
* Observability.

A traditional layered architecture can become difficult to maintain as the number of features increases because related functionality becomes distributed across many technical folders.

The application requires a structure that supports maintainability, testability, and independent evolution.

---

# Decision

The application will use a Clean Architecture foundation combined with Vertical Slice Architecture.

The solution will be organized into separate projects:

```
CloudOps.Api
CloudOps.Application
CloudOps.Domain
CloudOps.Infrastructure
CloudOps.Contracts
CloudOps.SharedKernel
```

Dependencies will follow the dependency inversion principle:

```
Api
 |
Application
 |
Domain
```

Infrastructure implementations will depend on application abstractions rather than the reverse.

---

# Project Responsibilities

## CloudOps.Api

Responsible for:

* HTTP endpoints.
* Request pipeline.
* Authentication.
* Middleware.
* Dependency injection.
* API documentation.

The API layer should not contain business logic.

---

## CloudOps.Application

Responsible for:

* Use cases.
* Commands.# ADR-0004: Clean Architecture and Vertical Slice Organization

- **Status:** Accepted
- **Date:** 2026-07-23
- **Category:** Architecture
- **Milestone:** 1
- **Decision Makers:** Project Owner

---

## Context

The CloudOps Event Platform is expected to evolve over multiple milestones while remaining maintainable, testable, and adaptable to future requirements.

The platform integrates multiple concerns including:

- HTTP APIs
- Background processing
- Messaging
- Persistence
- Infrastructure provisioning
- Observability

Without clear architectural boundaries, these concerns would become tightly coupled, making future enhancements increasingly difficult.

The project therefore requires an architectural approach that separates business logic from infrastructure while organizing functionality in a way that scales as new features are introduced.

---

## Decision

The platform adopts **Clean Architecture** for logical separation of concerns and **Vertical Slice Architecture** for feature organization.

Business rules remain independent of infrastructure technologies, while application functionality is organized by feature rather than technical layer.

The solution is divided into the following primary projects:

| Project | Responsibility |
|----------|----------------|
| **CloudOps.Api** | HTTP endpoints and request handling |
| **CloudOps.Application** | Application use cases and orchestration |
| **CloudOps.Domain** | Core domain models and business concepts |
| **CloudOps.Infrastructure** | AWS integrations and external services |
| **CloudOps.Worker** | Background event processing |
| **Infra** | AWS CDK infrastructure definitions |

Within each project, features are organized using a vertical slice approach where each feature contains its own endpoint, command models, handlers, and validation.

---

## Architectural Principles

### Dependency Rule

Dependencies always point inward.

The Domain layer has no dependencies.

The Application layer depends only on the Domain.

Infrastructure depends on the Application.

Presentation projects depend on the Application.

### Separation of Concerns

Each project has a clearly defined responsibility.

Business logic remains isolated from transport protocols, persistence, and cloud-specific implementations.

### Feature-Oriented Organization

Application functionality is grouped by business capability instead of technical artifact.

For example:

- Submit Event
- Process Event
- Health

rather than folders such as:

- Controllers
- Services
- Repositories

### Dependency Inversion

Application services communicate through abstractions.

Infrastructure provides concrete implementations through dependency injection.

### Replaceable Infrastructure

AWS implementations are treated as adapters that can be replaced without modifying business logic.

---

## Rationale

Clean Architecture provides long-term maintainability by protecting business rules from infrastructure changes.

Vertical Slice Architecture complements this by allowing each feature to evolve independently, reducing coupling between unrelated functionality.

Together, these approaches provide:

- Clear project boundaries
- Improved readability
- Easier testing
- Reduced coupling
- Better scalability as additional features are introduced

---

## Consequences

### Benefits

- Business logic remains infrastructure-independent.
- Features are easier to understand and maintain.
- Individual slices can evolve independently.
- Dependency injection remains straightforward.
- Infrastructure implementations remain replaceable.
- Supports incremental development through milestones.

### Trade-offs

- More projects and folders than smaller applications.
- Increased upfront architectural design.
- Additional abstractions require discipline to maintain.
- Navigation may initially be less familiar to developers accustomed to layered architectures.

---

## Alternatives Considered

### Traditional Layered Architecture

Rejected.

Organizing code primarily by technical layer (Controllers, Services, Repositories) tends to scatter feature implementations across multiple folders, increasing cognitive load as the application grows.

### Monolithic Feature Organization

Rejected.

Combining infrastructure, business logic, and presentation into a single project simplifies initial development but reduces maintainability and makes testing more difficult.

### Feature Folders Without Clean Architecture

Considered but not selected.

Feature folders improve organization but do not enforce dependency boundaries between business logic and infrastructure.

---

## Implementation Summary

As of **Milestone 4 (v0.4.0)**:

- HTTP endpoints are implemented using ASP.NET Core Minimal APIs.
- Features are organized into vertical slices.
- AWS services are accessed exclusively through Infrastructure implementations.
- Background processing executes in a dedicated Worker service.
- Infrastructure provisioning is isolated within the AWS CDK project.
- Observability concerns remain outside the Domain and Application layers.

This structure has supported the implementation of event ingestion, asynchronous processing, persistence, monitoring, and health endpoints without requiring architectural restructuring.

---

## Related ADRs

- ADR-0001 – Project Vision and Architectural Foundation
- ADR-0002 – Infrastructure as Code with AWS CDK
- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0005 – Event Ingestion API Design
- ADR-0007 – Dedicated Worker Service

---

## Status

This decision remains valid as of **Milestone 4 (v0.4.0)** and continues to guide the organization and dependency structure of the CloudOps Event Platform.
* Queries.
* Validation.
* Application workflows.
* Service interfaces.

This layer coordinates business operations.

---

## CloudOps.Domain

Responsible for:

* Entities.
* Value objects.
* Domain rules.
* Domain events.

The domain layer contains business concepts and has no dependency on external systems.

---

## CloudOps.Infrastructure

Responsible for:

* AWS integrations.
* Persistence implementations.
* Messaging implementations.
* External service communication.

Infrastructure details remain isolated from business logic.

---

## CloudOps.Contracts

Responsible for:

* API request models.
* API response models.
* Shared external contracts.

---

## CloudOps.SharedKernel

Responsible for:

* Shared abstractions.
* Common types.
* Cross-cutting primitives.

---

# Vertical Slice Architecture

Features will be organized around business capabilities rather than technical layers.

Example:

```
Features/

Events/

    SubmitEvent/

        Command

        Handler

        Validator

        Response
```

This keeps all components required for a single business capability together.

---

# Alternatives Considered

## Traditional Layered Architecture

Example:

```
Controllers
Services
Repositories
Models
```

Advantages:

* Familiar.
* Simple initially.

Disadvantages:

* Features become distributed.
* Changes often touch many folders.
* Navigation becomes difficult as the application grows.

---

## Microservices

Advantages:

* Independent deployment.
* Strong service boundaries.

Disadvantages:

* Additional operational complexity.
* Increased infrastructure requirements.
* Not justified for the current platform size.

---

# Consequences

## Positive

* Clear separation of responsibilities.
* Easier testing.
* Better maintainability.
* Supports future growth.
* Infrastructure can evolve independently.
* Features remain cohesive.

## Trade-offs

* More projects initially.
* Requires understanding architectural boundaries.
* More structure than a simple API project.

---

# Future Improvements

Potential future additions:

* MediatR-based command pipeline.
* FluentValidation integration.
* OpenTelemetry instrumentation.
* Feature-level integration tests.
* Automated architecture validation.

---

# Decision Summary

The CloudOps Event Platform adopts Clean Architecture combined with Vertical Slice Architecture to create a maintainable foundation for an event-driven cloud application.

The architecture prioritizes clear boundaries, testability, and long-term evolution over short-term simplicity.
