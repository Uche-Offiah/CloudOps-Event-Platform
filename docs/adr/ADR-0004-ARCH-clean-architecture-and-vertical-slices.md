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
* Commands.
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
