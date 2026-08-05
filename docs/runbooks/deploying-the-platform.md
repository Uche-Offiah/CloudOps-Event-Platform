# Runbook – Deploying the CloudOps Event Platform

**Applies To:** CloudOps Event Platform  
**Release:** v0.4.0

---

# Purpose

This runbook describes the end-to-end deployment procedure for the CloudOps Event Platform.

It covers infrastructure provisioning, application startup, deployment verification, and post-deployment validation.

Execute this runbook for:

- Initial environment provisioning
- New environment deployment
- Infrastructure updates
- Application releases
- Disaster recovery deployments

---

# Scope

This runbook covers deployment of:

- AWS Infrastructure (CDK)
- Amazon DynamoDB
- Amazon SQS
- Amazon CloudWatch
- Amazon SNS
- ASP.NET Core API
- Worker Service

---

# Prerequisites

Before deployment, verify:

- AWS CLI is installed and configured.
- .NET SDK is installed.
- AWS CDK CLI is installed.
- Appropriate AWS IAM permissions are available.
- Repository has been cloned.
- Required environment configuration is complete.

---

# Deployment Procedure

## Step 1 – Build the Solution

From the repository root:

```bash
dotnet build
```

Expected result:

```
Build succeeded.
```

Resolve any build errors before continuing.

---

## Step 2 – Deploy Infrastructure

Navigate to the Infrastructure project.

Deploy the CloudFormation stack.

```bash
cd infra
cdk deploy
```

Wait until deployment completes successfully.

Expected resources include:

- Amazon SQS Queue
- Amazon DynamoDB Table
- CloudWatch Dashboard
- CloudWatch Alarms
- Amazon SNS Topic

Record the stack outputs for later verification.

---

## Step 3 – Verify AWS Resources

Confirm the following resources exist:

### Amazon SQS

- Queue created
- Queue URL recorded

### DynamoDB

- Events table exists
- Partition key is `EventId`

### CloudWatch

Verify:

- Dashboard exists
- Metrics namespace exists
- Alarms are present

### Amazon SNS

Verify:

- Alerts topic exists
- Required subscriptions are configured

---

## Step 4 – Configure Application

Update application configuration using the deployment outputs.

Verify:

- AWS Region
- Queue URL
- DynamoDB table name
- SNS topic (if applicable)

---

## Step 5 – Start the API

Run:

```bash
dotnet run --project src/CloudOps.Api
```

Expected log:

```
Application started.
```

Verify:

```
GET /health
```

returns

```
HTTP 200 OK
```

---

## Step 6 – Start the Worker

Run:

```bash
dotnet run --project src/CloudOps.Worker
```

Expected log:

```
CloudOps Worker started.
```

The Worker should immediately begin long polling Amazon SQS.

---

## Step 7 – Submit a Test Event

Submit a valid event.

Example:

```bash
curl -X POST http://localhost:5000/events \
-H "Content-Type: application/json" \
-d '{
      "eventType":"OrderCreated",
      "source":"Deployment",
      "payload":{
        "orderId":"ORD-1001"
      }
}'
```

Expected response:

```
HTTP 202 Accepted
```

---

## Step 8 – Verify Processing

Confirm:

- Worker receives the message.
- Event is persisted.
- Message is deleted from SQS.
- DynamoDB contains the event.

---

## Step 9 – Verify Observability

Confirm:

- Structured logs are generated.
- CorrelationId appears in API and Worker logs.
- CloudWatch metrics update.
- Dashboard displays current values.
- Alarms remain healthy.

---

# Post-Deployment Checklist

Deployment is considered successful when:

- Infrastructure deployed successfully.
- API starts successfully.
- Worker starts successfully.
- Health endpoint responds.
- Test event is accepted.
- Worker processes the event.
- Event persists in DynamoDB.
- Queue is empty after processing.
- Metrics update.
- Dashboard displays data.
- Alarms remain in the OK state.

---

# Rollback

If deployment fails:

1. Stop the API.
2. Stop the Worker.
3. Review deployment logs.
4. Resolve configuration or infrastructure issues.
5. Redeploy the CloudFormation stack if necessary.
6. Restart the application components.
7. Repeat the verification steps.

---

# Troubleshooting

## CDK Deployment Fails

Verify:

- AWS credentials
- Bootstrap status
- IAM permissions
- Region configuration

---

## API Does Not Start

Verify:

- Application configuration
- Queue URL
- DynamoDB table name
- Build output

---

## Worker Does Not Receive Messages

Verify:

- Queue URL
- AWS credentials
- Worker logs
- IAM permissions

---

## DynamoDB Writes Fail

Verify:

- Table name
- IAM permissions
- Repository logs
- Conditional write configuration

---

## Dashboard Displays No Data

Verify:

- CloudWatch namespace
- Custom metrics
- Metric publishing
- Dashboard configuration

---

# References

## ADRs

- ADR-0002 – Infrastructure as Code with AWS CDK
- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0007 – Dedicated Worker Service
- ADR-0009 – Event Processing Pipeline Architecture
- ADR-0010 – Observability Strategy

## Related Runbooks

- Testing Worker SQS Connectivity
- Observability Validation
- Duplicate Event Verification

## Related Learning Notes

- Milestone 1 – Infrastructure Foundation
- Milestone 4 – Observability and Operational Readiness