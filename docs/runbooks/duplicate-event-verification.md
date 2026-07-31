# Runbook – Duplicate Event Verification

**Applies To:** CloudOps Event Platform  
**Release:** v0.4.0

---

# Purpose

This runbook verifies that the CloudOps Event Platform correctly handles duplicate event processing.

The platform is designed to be idempotent. If the same event is delivered multiple times, only a single copy should be persisted while duplicate deliveries are detected, logged, and reflected in operational metrics.

Execute this runbook after:

- Changes to event persistence logic
- Changes to Worker processing
- Changes to DynamoDB repository implementation
- Changes to duplicate detection logic
- Major application releases

---

# Scope

This runbook validates:

- Duplicate message handling
- DynamoDB conditional writes
- Worker processing behavior
- Structured logging
- CloudWatch custom metrics
- CloudWatch alarms

---

# Prerequisites

Before beginning, verify:

- API is running.
- Worker is running.
- Amazon SQS queue exists.
- DynamoDB table exists.
- CloudWatch metrics are enabled.
- CloudWatch alarms have been deployed.

---

# Background

Amazon SQS provides **at-least-once delivery**.

A message may be delivered more than once due to retries, visibility timeout expiration, or transient failures.

The platform prevents duplicate persistence by using a DynamoDB conditional write:

```text
attribute_not_exists(EventId)
```

If a duplicate EventId is detected, DynamoDB throws a `ConditionalCheckFailedException`.

The Worker records the duplicate, increments the `DuplicateEvents` metric, and continues processing without creating another database record.

---

# Procedure

## Step 1 – Submit an Initial Event

Submit a valid event using the API.

Example:

```bash
curl -X POST http://localhost:5000/events \
-H "Content-Type: application/json" \
-d '{
      "eventType":"OrderCreated",
      "source":"DuplicateRunbook",
      "payload":{
          "orderId":"ORD-1001"
      }
}'
```

Expected response:

```
HTTP 202 Accepted
```

Record the returned EventId.

---

## Step 2 – Verify Initial Processing

Confirm:

- Worker processes the message.
- DynamoDB contains one record.
- Queue message is deleted.
- EventsProcessed metric increments.

---

## Step 3 – Generate a Duplicate

Using the recorded EventId, publish an identical EventEnvelope directly to Amazon SQS.

This simulates duplicate delivery from the messaging infrastructure.

---

## Step 4 – Observe Worker Logs

The Worker should process the duplicate message.

Expected log sequence:

```
Processing EventId ...

Duplicate EventId detected.

Processing completed.
```

No unhandled exception should terminate processing.

---

## Step 5 – Verify DynamoDB

Query the Events table.

Expected result:

- Exactly one record exists.
- No additional item has been created.
- Original event data remains unchanged.

---

## Step 6 – Verify CloudWatch Metrics

Confirm:

| Metric | Expected Result |
|---------|-----------------|
| EventsProcessed | Does not increase for the duplicate |
| DuplicateEvents | Increments by one |
| EventsFailed | Does not increment |

---

## Step 7 – Verify Dashboard

Open the CloudWatch Dashboard.

Confirm:

- DuplicateEvents widget reflects the additional duplicate.
- Processing metrics remain consistent.
- Dashboard reports no unexpected failures.

---

## Step 8 – Verify Alarm Behavior

Review the DuplicateEvents alarm.

Expected result:

- Alarm remains **OK** if the configured threshold is not exceeded.
- Alarm transitions to **ALARM** only when the threshold is exceeded.

If alarm actions are configured:

- Verify an Amazon SNS notification is published.

---

# Verification Checklist

The validation is successful when:

- Initial event is persisted.
- Duplicate event is detected.
- Only one DynamoDB record exists.
- DuplicateEvents metric increments.
- EventsProcessed is unchanged.
- EventsFailed remains unchanged.
- Worker continues processing normally.
- Dashboard reflects duplicate activity.
- Alarm behavior matches the configured threshold.

---

# Expected Results

A successful validation confirms that the platform:

- Supports idempotent event processing.
- Prevents duplicate persistence.
- Detects duplicate deliveries.
- Preserves original event data.
- Publishes operational telemetry for duplicate activity.
- Continues processing without interruption.

---

# Troubleshooting

## Duplicate Record Exists

Possible causes:

- Conditional write removed.
- Incorrect partition key.
- Repository implementation changed.

Verify:

- `ConditionExpression`
- DynamoDB schema
- Repository implementation

---

## DuplicateEvents Metric Does Not Increase

Possible causes:

- Metric publisher not executed.
- Duplicate exception not handled correctly.
- CloudWatch namespace mismatch.

Verify:

- Worker logs.
- Metric publishing logic.
- CloudWatch configuration.

---

## Worker Stops Processing

Possible causes:

- Duplicate exception escapes the processing pipeline.
- Repository exception is not handled correctly.

Verify:

- Worker logs.
- ProcessingResult handling.
- Repository exception handling.

---

## Alarm Does Not Trigger

Possible causes:

- Threshold too high.
- Wrong metric configured.
- Evaluation periods not satisfied.

Verify:

- Alarm configuration.
- Metric dimensions.
- CloudWatch evaluation settings.

---

# References

## ADRs

- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0007 – Dedicated Worker Service
- ADR-0008 – Amazon SQS Long Polling Strategy
- ADR-0009 – Event Processing Pipeline Architecture
- ADR-0010 – Observability Strategy

## Related Runbooks

- Deploying the CloudOps Event Platform
- Testing Worker SQS Connectivity
- Observability Validation

## Related Learning Notes

- Milestone 3 – Asynchronous Event Processing
- Milestone 4 – Observability and Operational Readiness