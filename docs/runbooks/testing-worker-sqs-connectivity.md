# Testing Worker Connectivity to Amazon SQS

## Prerequisites

- Infrastructure deployed.
- API running.  
- Worker configured with the correct queue URL.

## Procedure

1. Start the API.
    ```dotnet run --project src/CloudOps.Api

2. Submit a POST `/events` request.
    ```curl -X POST "http://localhost:5148/events" \
  -H "Content-Type: application/json" \
  -d '{
    "source": "curl-test",
    "eventType": "OrderCreated",
	"version": 1,
    "payload": "{\"orderId\":\"ORD-1001\",\"customerId\":\"CUST-2001\",\"amount\":149.99}"
}'

3. Start the worker.
    ```dotnet run --project src/CloudOps.Worker

4. Observe the logs.

## Expected Result

The worker logs:

- Worker started
- Number of received messages
- Message IDs

Because messages are not deleted in this milestone, they should become visible again after the configured visibility timeout.