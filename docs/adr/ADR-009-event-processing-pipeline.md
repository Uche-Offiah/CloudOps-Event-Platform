# Event Processing Pipeline

## Status

Accepted

## Context

The CloudOps Event Platform accepts operational events through an HTTP API and processes them asynchronously.

The processing pipeline must:

- decouple producers from consumers
- provide durable message delivery
- support future replay
- enable operational monitoring
- persist the complete event envelope

## Decision

The platform implements the following pipeline:

Client
→ API
→ Amazon SQS
→ Worker
→ EventMessageProcessor
→ DynamoDB

The Worker is implemented as a .NET BackgroundService.

A new dependency injection scope is created for each polling iteration using IServiceScopeFactory to ensure correct scoped service lifetimes.

The complete EventEnvelope is stored in DynamoDB.

The SQS message is deleted only after successful persistence.

## Consequences

Advantages

- asynchronous processing
- scalable worker architecture
- durable event storage
- replay capability
- clean separation of concerns

Trade-offs

- eventual consistency
- duplicate detection deferred to Reliability milestone