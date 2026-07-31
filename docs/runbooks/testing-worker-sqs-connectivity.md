# Runbook – Testing Worker SQS Connectivity

**Applies To:** CloudOps Event Platform  
**Release:** v0.4.0

---

# Purpose

This runbook verifies that the Worker service can successfully consume messages from Amazon SQS, process events, persist them to Amazon DynamoDB, and remove processed messages from the queue.

This procedure should be executed after:

- Initial deployment
- Infrastructure changes
- Worker configuration updates
- AWS credential changes
- Queue configuration changes

---

# Scope

This runbook validates the complete Worker processing path:

```
Client
   │
   ▼
HTTP API
   │
   ▼
Amazon SQS
   │
   ▼
Worker
   │
   ▼
DynamoDB
```

---

# Prerequisites

Before beginning, verify:

- AWS infrastructure has been deployed successfully.
- Worker service is running.
- API is running.
- Amazon SQS queue exists.
- Amazon DynamoDB table exists.
- AWS credentials are valid.
- CloudWatch logging is enabled.

---

# Procedure

## Step 1 – Verify Worker Startup

Start the Worker application.

Expected log output:

```
CloudOps Worker started.
```

The Worker should begin polling Amazon SQS immediately.

---

## Step 2 – Submit a Test Event

Submit an event using the API.

Example:

```bash
curl -X POST http://localhost:5000/events \
  -H "Content-Type: application/json" \
  -d '{
        "eventType":"OrderCreated",
        "source":"Runbook",
        "payload":{
          "orderId":"ORD-1001",
          "customerId":"CUST-2001",
          "amount":149.99
        }
      }'
```

Expected response:

```
HTTP 202 Accepted
```

Record the returned:

- EventId
- CorrelationId (if exposed)
- Accepted timestamp

---

## Step 3 – Verify Queue Processing

Observe Worker logs.

Expected sequence:

```
Received 1 message(s)

Processing EventId ...

Persisting EventId ...

Successfully persisted EventId ...

Deleted MessageId ...
```

No exceptions should be reported.

---

## Step 4 – Verify DynamoDB

Open the DynamoDB table.

Confirm a new item exists.

Verify:

- EventId
- EventType
- Source
- CorrelationId
- Payload
- OccurredAtUtc
- Version

match the submitted request.

---

## Step 5 – Verify Queue

Open Amazon SQS.

Confirm:

- Queue depth returns to zero.
- No messages remain in flight.
- No messages are visible.

---

## Step 6 – Verify CloudWatch Logs

Confirm structured log entries exist for:

API:

- Event submission

Worker:

- Message reception
- Processing
- Persistence
- Deletion

Verify the CorrelationId is consistent throughout the processing lifecycle.

---

# Verification Checklist

The test is successful when all of the following are true:

- API returns HTTP 202.
- Message reaches Amazon SQS.
- Worker receives the message.
- Event is persisted to DynamoDB.
- Message is deleted from Amazon SQS.
- No unexpected exceptions occur.
- CorrelationId is preserved across logs.

---

# Expected Results

A successful test demonstrates:

- API connectivity
- Amazon SQS connectivity
- Worker polling
- Event processing
- DynamoDB persistence
- Queue acknowledgement
- Structured logging

---

# Troubleshooting

## Worker Does Not Receive Messages

Possible causes:

- Incorrect Queue URL
- Worker not running
- Invalid AWS credentials
- IAM permission issues

Verify:

- AwsOptions
- Queue URL
- IAM permissions
- Worker logs

---

## Message Remains in Queue

Possible causes:

- Processing exception
- Visibility timeout expiration
- DeleteMessageAsync not executed

Verify:

- Worker logs
- Exception details
- ProcessingResult status

---

## DynamoDB Contains No Item

Possible causes:

- Table name configuration
- Conditional write failure
- IAM permissions

Verify:

- EventsTableName configuration
- DynamoDB permissions
- Repository logs

---

## Duplicate Event Logged

This is expected if the same EventId is processed more than once.

Verify that:

- DynamoDB contains only one item.
- DuplicateEvents metric increments.
- No data corruption occurs.

---

# References

### ADRs

- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0007 – Dedicated Worker Service
- ADR-0008 – Amazon SQS Long Polling Strategy
- ADR-0009 – Event Processing Pipeline Architecture

### Learning Notes

- Milestone 3 – Asynchronous Event Processing
- Milestone 4 – Observability and Operational Readiness