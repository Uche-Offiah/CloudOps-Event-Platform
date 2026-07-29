# Milestone 3.3 – Amazon SQS Reception

## Objective

Verify that the worker can connect to Amazon SQS and receive messages using long polling.

## Achievements

- Configured Amazon SQS client.
- Implemented long polling.
- Received messages successfully.
- Logged message metadata.
- Preserved messages for observation by not deleting them.

## Lessons Learned

- Long polling reduces unnecessary API calls.
- Visibility timeout prevents immediate reprocessing.
- Processing and deletion should remain separate concerns.

## Next Steps

- Deserialize the event envelope.
- Validate the message.
- Persist the event to DynamoDB.
- Delete the message only after successful processing.

# Milestone 3.4 – Process SQS Queue and Presist to DynamoDB

## Amazon SQS

- Long polling reduces empty receives.
- Visibility Timeout prevents duplicate processing while a message is in flight.
- Messages should only be deleted after successful processing.

## BackgroundService

Hosted services are singletons.

Scoped dependencies must be resolved using IServiceScopeFactory.

## DynamoDB

PutItemAsync stores the complete event envelope.

The payload is stored as JSON to preserve the original event.

## Event Processing

Application Layer

- EventMessageProcessor

Infrastructure Layer

- DynamoDbEventRepository

Worker orchestration remains separate from business logic.