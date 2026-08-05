# ADR-0012: Containerization Strategy

**Status:** Accepted

## Context

The CloudOps Event Platform currently runs directly using the .NET SDK during development.

As the application grows, differences between developer environments can introduce inconsistencies that affect reliability and onboarding.

Containerization provides a consistent runtime environment and prepares the platform for future deployment targets such as Amazon ECS, Kubernetes, or other container orchestration platforms.

The solution contains two independently deployable services:

- CloudOps.Api
- CloudOps.Worker

Each service should be independently containerized while sharing the same build approach.

## Decision

The platform will adopt Docker for local development and future deployment.

Each deployable service will have its own multi-stage Dockerfile.

A Docker Compose configuration will orchestrate the API and Worker locally.

The Docker images will:

- Use official Microsoft .NET images.
- Separate build and runtime stages.
- Minimize runtime image size.
- Support environment variable configuration.
- Remain independent of deployment infrastructure.

Container images will not contain environment-specific configuration.

## Consequences

### Positive

- Consistent developer environments.
- Simplified onboarding.
- Faster transition to cloud container platforms.
- Smaller production images through multi-stage builds.
- Improved deployment portability.

### Negative

- Additional build artifacts to maintain.
- Slight increase in local development complexity.

## Alternatives Considered

### Continue Using Local .NET Execution

Rejected because it does not provide environment consistency or deployment portability.

### Single Dockerfile for Entire Solution

Rejected because the API and Worker have different runtime responsibilities and deployment lifecycles.

## Related ADRs

- ADR-0002 – Infrastructure as Code with AWS CDK
- ADR-0007 – Dedicated Worker Service
- ADR-0011 – Continuous Integration Strategy