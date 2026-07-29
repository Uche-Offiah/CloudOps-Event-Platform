# Milestone 3 Verification

## Infrastructure

- DynamoDB table exists
- SQS queue exists
- DLQ exists

## API

- POST returns 202

## Worker

- Receives messages
- Processes messages
- Deletes processed messages

## Persistence

- Event stored in DynamoDB

## Messaging

- Queue empty after processing
- DLQ empty

## Logs

Expected log flow

Submit Event

↓

Publish SQS

↓

Receive Message

↓

Deserialize

↓

Persist Event

↓

Delete Message