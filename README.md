# CloudOps Event Platform

![.NET](https://img.shields.io/badge/.NET-9-blue)
![AWS](https://img.shields.io/badge/AWS-Cloud-orange)
![CDK](https://img.shields.io/badge/CDK-v2-green)
![Release](https://img.shields.io/badge/Release-v0.4.0-success)

> A production-oriented event-driven platform built with **ASP.NET Core**, **Amazon Web Services (AWS)**, and **AWS Cloud Development Kit (CDK)** that demonstrates modern cloud-native architecture, asynchronous messaging, Infrastructure as Code (IaC), distributed processing, and operational observability.

---

## Current Release

**Version:** **v0.4.0**

**Status**

✅ Milestone 4 Complete – **Observability and Operational Readiness**

---

# Overview

The **CloudOps Event Platform** is a cloud-native application that demonstrates how modern distributed systems ingest, process, persist, and monitor business events using AWS managed services.

The platform exposes a REST API for accepting events, publishes accepted requests to Amazon SQS, processes messages asynchronously using a dedicated Worker Service, and persists successfully processed events into Amazon DynamoDB.

Beginning with Milestone 4, the platform also provides production-oriented operational capabilities including structured logging, end-to-end correlation identifiers, CloudWatch metrics, dashboards, alarms, Amazon SNS notifications, and health monitoring.

The project emphasizes engineering best practices including:

- Clean Architecture
- Domain-driven separation of concerns
- Infrastructure as Code
- Event-driven communication
- Idempotent message processing
- Operational observability
- Comprehensive engineering documentation

Rather than focusing only on application functionality, this repository demonstrates how cloud-native systems are designed, deployed, monitored, and maintained.

---

# Architecture

The platform processes events asynchronously while collecting operational telemetry throughout the processing pipeline.

```text
                           Amazon CloudWatch
                   ┌─────────────────────────────┐
                   │ Structured Logs             │
                   │ Custom Metrics              │
                   │ Dashboard                   │
                   │ Alarms                      │
                   └──────────────▲──────────────┘
                                  │
                                  │
                          Amazon SNS Alerts
                                  ▲
                                  │
                                  │
Client
   │
   ▼
┌────────────────────┐
│ ASP.NET Core API   │
│                    │
│ • Validation       │
│ • Correlation ID   │
│ • Logging          │
└─────────┬──────────┘
          │
          ▼
┌────────────────────┐
│ Amazon SQS         │
│ Event Queue        │
└─────────┬──────────┘
          │
          ▼
┌────────────────────┐
│ Worker Service     │
│                    │
│ • Long Polling     │
│ • Processing       │
│ • Metrics          │
│ • Logging          │
└─────────┬──────────┘
          │
          ▼
┌────────────────────┐
│ Amazon DynamoDB    │
│ Events Table       │
└────────────────────┘
```

---

# Core Features

The platform currently includes the following capabilities.

## Event Ingestion

- Minimal ASP.NET Core API
- Request validation
- JSON payload handling
- Correlation ID generation
- Asynchronous event publishing
- HTTP 202 Accepted responses

---

## Messaging

- Amazon SQS integration
- Long polling
- Visibility timeout configuration
- Reliable asynchronous communication
- Message acknowledgement
- Retry support

---

## Event Processing

- Dedicated Worker Service
- Background processing
- Event deserialization
- Repository abstraction
- Idempotent persistence
- Duplicate event detection

---

## Persistence

- Amazon DynamoDB
- Conditional writes
- Event versioning
- JSON payload storage
- Repository pattern

---

## Observability

- Structured logging
- Correlation ID propagation
- CloudWatch custom metrics
- CloudWatch Dashboard
- CloudWatch Alarms
- Amazon SNS notifications
- API health endpoint

---

## Infrastructure

- AWS CDK
- Infrastructure as Code
- Environment-aware configuration
- Automated resource provisioning
- Reusable infrastructure constructs
- CloudFormation deployment

---

# Operational Capabilities

Milestone 4 introduced comprehensive operational monitoring across the platform.

Current capabilities include:

| Capability | Description |
|------------|-------------|
| Structured Logging | Consistent log templates with contextual properties |
| Correlation IDs | End-to-end tracing across API and Worker |
| Custom Metrics | CloudWatch operational metrics |
| Dashboard | Real-time operational visibility |
| Alarms | Automatic detection of operational issues |
| SNS Notifications | Alarm notifications for operators |
| Health Endpoint | Deployment and readiness verification |

These capabilities transform the platform from a functioning distributed application into an operationally observable cloud-native system.


# Technology Stack

The CloudOps Event Platform combines modern .NET development practices with AWS managed services to build a scalable, event-driven architecture.

## Application

- ASP.NET Core Minimal API
- C#
- .NET

## AWS Services

- Amazon SQS
- Amazon DynamoDB
- Amazon CloudWatch
- Amazon SNS

## Infrastructure

- AWS CDK
- AWS CloudFormation

## Architectural Patterns

- Clean Architecture
- Repository Pattern
- Dependency Injection
- Background Worker Pattern
- Event-Driven Architecture
- Infrastructure as Code (IaC)

---

# Repository Structure

The solution is organized to clearly separate application concerns, infrastructure, documentation, and testing.

```text
CloudOps-Event-Platform
│
├── docs
│   ├── adr
│   ├── architecture
│   ├── learning
│   ├── releases
│   └── runbooks
│
├── infra
│   ├── Common
│   ├── Config
│   ├── Constructs
│   ├── Models
│   ├── Stacks
│   └── Program.cs
│
├── src
│   ├── CloudOps.Api
│   ├── CloudOps.Application
│   ├── CloudOps.Domain
│   ├── CloudOps.Infrastructure
│   └── CloudOps.Worker
│
├── tests
│
└── README.md
```

---

# Solution Architecture

The application follows the principles of **Clean Architecture**, separating business logic from infrastructure concerns.

```text
Presentation
    │
    ▼
Application
    │
    ▼
Domain
    ▲
    │
Infrastructure
```

### CloudOps.Api

Hosts the ASP.NET Core Minimal API responsible for accepting event submissions and exposing operational endpoints.

Responsibilities include:

- HTTP endpoints
- Request validation
- Dependency injection configuration
- Correlation ID initialization
- Health endpoint

---

### CloudOps.Application

Contains the application's use cases and orchestration logic.

Responsibilities include:

- Commands
- Handlers
- Interfaces
- Validation
- Event processing workflows

The Application layer depends only on abstractions and contains no AWS-specific implementations.

---

### CloudOps.Domain

Represents the core business model.

Responsibilities include:

- Event models
- Domain contracts
- Shared business concepts

The Domain layer has no dependencies on infrastructure frameworks.

---

### CloudOps.Infrastructure

Implements integrations with external services.

Responsibilities include:

- Amazon SQS publishing
- Amazon DynamoDB persistence
- CloudWatch metric publishing
- AWS configuration
- Repository implementations

Infrastructure fulfills interfaces defined by the Application layer.

---

### CloudOps.Worker

Hosts the background service responsible for asynchronous message processing.

Responsibilities include:

- Amazon SQS long polling
- Message processing
- Event persistence
- Metric publication
- Structured logging

---

### Infrastructure (AWS CDK)

Defines all AWS resources using Infrastructure as Code.

Provisioned resources include:

- Amazon SQS
- Amazon DynamoDB
- CloudWatch Dashboard
- CloudWatch Alarms
- Amazon SNS Topic

---

# Documentation

Engineering documentation is organized by purpose to separate architectural decisions, implementation knowledge, operational procedures, and release history.

| Folder | Purpose |
|---------|---------|
| **docs/adr** | Architecture Decision Records documenting significant technical decisions |
| **docs/architecture** | High-level architecture and technical reference documentation |
| **docs/learning** | Lessons learned and engineering insights captured after each milestone |
| **docs/releases** | Release notes describing completed milestones and delivered capabilities |
| **docs/runbooks** | Operational procedures for deployment, verification, troubleshooting, and production support |

---

# Engineering Documentation

Documentation is treated as a first-class deliverable alongside the source code.

Current documentation includes:

## Architecture Decision Records (ADRs)

Ten ADRs capture the major architectural decisions that shaped the platform, including:

- Platform architecture
- Infrastructure as Code
- Amazon SQS messaging
- DynamoDB persistence
- Worker Service
- Event processing pipeline
- Observability strategy

---

## Architecture Documentation

Technical reference documents describe the overall system design and operational architecture.

Current references include:

- Observability Architecture

---

## Learning Notes

Each completed milestone includes an engineering retrospective documenting:

- Objectives
- Technical challenges
- Design decisions
- Lessons learned
- Future improvements

Current learning notes:

- Milestone 1 – Infrastructure Foundation
- Milestone 2 – Event Ingestion Foundation
- Milestone 3 – Asynchronous Event Processing
- Milestone 4 – Observability and Operational Readiness

---

## Operational Runbooks

Operational procedures provide repeatable guidance for deployment, validation, and troubleshooting.

Current runbooks include:

- Deploying the CloudOps Event Platform
- Testing Worker SQS Connectivity
- Duplicate Event Verification
- Observability Validation

These runbooks ensure the platform can be deployed, operated, and validated consistently across environments.


# Milestone Progress

The project is being developed incrementally using milestone-based delivery. Each milestone introduces a coherent set of capabilities while building on the previous release.

| Version | Milestone | Status |
|----------|-----------|:------:|
| **v0.1.0** | Infrastructure Foundation | ✅ Complete |
| **v0.2.0** | Event Ingestion Foundation | ✅ Complete |
| **v0.3.0** | Asynchronous Event Processing | ✅ Complete |
| **v0.4.0** | Observability and Operational Readiness | ✅ Complete |
| **v0.5.0** | CI/CD and Production Delivery | ⬜ Planned |
| **v0.6.0** | Security and Reliability | ⬜ Planned |

---

# Getting Started

## Prerequisites

Install the following tools before building the solution.

### Development

- .NET SDK
- Git

### AWS

- AWS CLI
- AWS CDK
- AWS Account
- Configured AWS credentials

---

## Clone the Repository

```bash
git clone <repository-url>

cd CloudOps-Event-Platform
```

---

## Build the Solution

```bash
dotnet build
```

Expected output:

```text
Build succeeded.
```

---

## Deploy the Infrastructure

Navigate to the Infrastructure project.

```bash
cd infra
```

Deploy the AWS resources.

```bash
cdk deploy
```

The deployment provisions:

- Amazon DynamoDB Table
- Amazon SQS Queue
- CloudWatch Dashboard
- CloudWatch Alarms
- Amazon SNS Topic

Record the CloudFormation outputs after deployment.

---

## Configure the Application

Update the application configuration with the deployment outputs.

Verify:

- AWS Region
- Queue URL
- DynamoDB Table Name
- AWS Credentials

---

## Run the API

```bash
dotnet run --project src/CloudOps.Api
```

The API should start successfully.

Verify the health endpoint.

```
GET /health
```

Expected response:

```text
HTTP 200 OK
```

---

## Run the Worker

In a separate terminal:

```bash
dotnet run --project src/CloudOps.Worker
```

Expected log:

```text
CloudOps Worker started.
```

The Worker immediately begins long polling Amazon SQS.

---

## Submit a Test Event

Submit an event using any HTTP client.

Example:

```http
POST /events
```

Example request body:

```json
{
  "eventType": "OrderCreated",
  "source": "README",
  "payload": {
    "orderId": "ORD-1001"
  }
}
```

Expected response:

```text
HTTP 202 Accepted
```

---

## Verify Processing

Confirm the following:

- Event accepted by the API
- Message appears in Amazon SQS
- Worker processes the message
- Event persisted to DynamoDB
- Message removed from the queue

---

# Operational Validation

After deployment, validate the platform using the operational runbooks located in:

```text
docs/runbooks/
```

Recommended execution order:

1. Deploying the CloudOps Event Platform
2. Testing Worker SQS Connectivity
3. Duplicate Event Verification
4. Observability Validation

Executing these runbooks verifies both application functionality and operational readiness.

---

# Health Monitoring

The platform exposes a lightweight health endpoint.

```
GET /health
```

Expected response:

```text
HTTP 200 OK
```

The endpoint is intended for:

- Deployment verification
- Readiness checks
- Operational monitoring
- Future load balancer health probes

---

# Monitoring

Operational visibility is provided through Amazon CloudWatch.

Current monitoring capabilities include:

- Structured application logs
- CloudWatch custom metrics
- CloudWatch Dashboard
- CloudWatch Alarms
- Amazon SNS notifications

Published metrics include:

| Metric | Description |
|---------|-------------|
| EventsAccepted | Successfully accepted API requests |
| EventsProcessed | Successfully processed Worker events |
| DuplicateEvents | Duplicate event detections |
| EventsFailed | Failed event processing |

These metrics provide insight into platform throughput, reliability, and operational health.

---

# Verification Checklist

A successful deployment should satisfy the following checklist.

| Verification | Status |
|--------------|--------|
| Infrastructure deployed | ✅ |
| API running | ✅ |
| Worker running | ✅ |
| Health endpoint responding | ✅ |
| Event accepted | ✅ |
| Worker processed event | ✅ |
| Event persisted | ✅ |
| Queue empty after processing | ✅ |
| Metrics published | ✅ |
| Dashboard populated | ✅ |
| Alarms healthy | ✅ |

Once all checks pass, the platform is considered successfully deployed and operational.

# Roadmap

The CloudOps Event Platform is being developed incrementally, with each milestone building upon a stable architectural foundation.

## Completed

### ✅ Milestone 1 – Infrastructure Foundation

- AWS CDK solution
- Amazon SQS
- Amazon DynamoDB
- Infrastructure as Code
- Environment-aware configuration
- Resource naming conventions

---

### ✅ Milestone 2 – Event Ingestion Foundation

- ASP.NET Core Minimal API
- Event validation
- Event publishing
- Repository abstractions
- Dependency injection
- Initial architecture documentation

---

### ✅ Milestone 3 – Asynchronous Event Processing

- Dedicated Worker Service
- Amazon SQS long polling
- Event persistence
- Idempotent processing
- Duplicate event detection
- Processing pipeline

---

### ✅ Milestone 4 – Observability and Operational Readiness

- Structured logging
- Correlation ID propagation
- CloudWatch custom metrics
- CloudWatch Dashboard
- CloudWatch Alarms
- Amazon SNS notifications
- Health endpoint
- Operational runbooks
- Architecture documentation
- Standardized ADRs and learning notes

---

## Planned

### 🚀 Milestone 5 – CI/CD and Production Delivery

Planned enhancements include:

- GitHub Actions CI/CD
- Automated builds
- Automated testing
- Docker containerization
- Deployment automation
- Release automation
- Quality gates
- Static code analysis

---

### 🔒 Milestone 6 – Security and Reliability

Planned enhancements include:

- AWS Secrets Manager
- IAM policy hardening
- Dead Letter Queue (DLQ)
- Retry strategies
- Resilience testing
- Enhanced monitoring
- Log retention policies

---

### 📈 Future Enhancements

Potential future capabilities include:

- OpenTelemetry
- AWS X-Ray
- Distributed tracing
- Service Level Indicators (SLIs)
- Service Level Objectives (SLOs)
- Error budgets
- Event replay
- Multi-environment deployments
- Kubernetes support
- EventBridge integration

---

# Design Principles

The CloudOps Event Platform is guided by a set of engineering principles intended to promote maintainability, scalability, and operational excellence.

## Clean Architecture

Business logic remains independent of infrastructure concerns through clear separation of responsibilities.

---

## Asynchronous Communication

Services communicate using durable messaging to reduce coupling and improve scalability.

---

## Infrastructure as Code

Cloud resources are provisioned using AWS CDK to ensure deployments are repeatable, version-controlled, and consistent across environments.

---

## Idempotent Processing

Duplicate event deliveries are expected in distributed systems.

The platform is designed to process events safely without creating duplicate data.

---

## Observability by Design

Logging, metrics, dashboards, alarms, and health monitoring are integrated into the platform architecture rather than added after implementation.

---

## Operational Readiness

Every feature should be deployable, monitorable, and verifiable using documented operational procedures.

---

## Documentation as Code

Documentation evolves alongside the source code and is maintained with the same discipline as the implementation.

The repository includes:

- Architecture Decision Records
- Architecture documentation
- Learning Notes
- Operational Runbooks
- Release Notes

---

# Contributing

This repository was created as a cloud engineering portfolio project and learning platform.

Contributions, suggestions, and constructive feedback are welcome.

When contributing:

- Follow the existing architectural patterns.
- Keep documentation synchronized with implementation changes.
- Record significant architectural decisions using ADRs.
- Update runbooks where operational behavior changes.
- Maintain consistent coding standards and naming conventions.

---

# License

This project is provided for educational and portfolio purposes.

It demonstrates practical cloud engineering techniques using:

- ASP.NET Core
- Amazon Web Services (AWS)
- AWS Cloud Development Kit (CDK)

You are welcome to study, reference, and adapt the implementation for learning purposes.

---

# Acknowledgements

This project demonstrates the application of modern cloud engineering practices, including event-driven architecture, Infrastructure as Code, operational observability, and engineering documentation.

It has been developed incrementally through milestone-based delivery, with each milestone introducing production-oriented capabilities while maintaining a strong architectural foundation.

The repository is intended to showcase not only application development, but also the engineering practices required to build, operate, and evolve cloud-native systems.