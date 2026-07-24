# Testing Worker Connectivity to Amazon SQS

## Prerequisites

- Infrastructure deployed.
- API running.
- Worker configured with the correct queue URL.

## Procedure

1. Start the API.
2. Submit a POST `/events` request.
3. Start the worker.
4. Observe the logs.

## Expected Result

The worker logs:

- Worker started
- Number of received messages
- Message IDs

Because messages are not deleted in this milestone, they should become visible again after the configured visibility timeout.