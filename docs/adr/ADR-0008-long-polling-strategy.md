# ADR-0008: Amazon SQS Long Polling Strategy

## Status

Accepted

## Context

The worker continuously polls Amazon SQS for new events. Short polling increases the number of empty responses, resulting in unnecessary API calls and higher costs.

## Decision

The worker uses Amazon SQS long polling with a wait time of 20 seconds.

Configuration:

- WaitTimeSeconds = 20
- MaxNumberOfMessages = 5
- VisibilityTimeout = 30

## Consequences

### Positive

- Reduces empty responses.
- Lowers SQS API costs.
- Improves worker efficiency.

### Negative

- Worker shutdown may wait for an in-flight receive request to complete unless cancellation is triggered.