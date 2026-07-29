# ADR-0006: Canonical Event Envelope

## Status

Accepted

## Context

Multiple producers and consumers will exchange events.

A consistent message structure is required to support logging, tracing, versioning, and future schema evolution.

## Decision

Every event published by the platform must use the following envelope:

```json
{
  "eventId": "...",
  "eventType": "...",
  "source": "...",
  "occurredAtUtc": "...",
  "correlationId": "...",
  "version": 1,
  "payload": { }
}
```