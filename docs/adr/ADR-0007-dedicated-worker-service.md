# ADR-0007: Dedicated Worker Service

## Status

Accepted

## Context

The platform receives events through an HTTP API and processes them asynchronously.

A decision is required regarding where asynchronous message processing should execute.

## Decision

A dedicated .NET Worker Service will be created to process Amazon SQS messages.

The worker will run independently from the HTTP API and communicate only through Amazon SQS.

## Consequences

### Positive

- Independent scaling
- Improved fault isolation
- Clear separation of responsibilities
- Easier future deployment to ECS, App Runner, or Kubernetes
- Supports multiple worker instances

### Negative

- Additional deployment artifact
- More operational components to monitor

## Alternatives Considered

### BackgroundService inside CloudOps.Api

Rejected because it couples HTTP request handling with long-running background processing and prevents independent scaling.