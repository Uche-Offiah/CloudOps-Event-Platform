# ADR-0002: Infrastructure as Code with AWS CDK

- **Status:** Accepted
- **Date:** 2026-07-21
- **Category:** Infrastructure
- **Milestone:** 0
- **Decision Makers:** Project Owner

---

## Context

The CloudOps Event Platform provisions multiple AWS resources to support event ingestion, processing, persistence, notification, and operational monitoring.

These resources include, but are not limited to:

- Amazon SQS
- Amazon DynamoDB
- Amazon SNS
- Amazon CloudWatch
- IAM Roles and Policies

Managing these resources manually would introduce configuration drift, reduce deployment repeatability, and make environment recreation difficult.

The platform also targets a development workflow where cloud resources may be provisioned and destroyed regularly to reduce AWS costs while preserving the infrastructure definition in source control.

---

## Decision

Infrastructure will be defined using **AWS Cloud Development Kit (AWS CDK)** in C#.

All cloud resources required by the platform will be provisioned, updated, and removed through CDK deployments rather than manual configuration.

Infrastructure code will reside alongside the application source code and evolve through the same milestone-based development process.

Resource naming, tagging, and environment configuration will be centralized to ensure consistency across deployments.

---

## Architectural Principles

### Infrastructure as Source Code

Infrastructure definitions are version-controlled and reviewed alongside application code.

### Repeatable Deployments

Every deployment should produce equivalent infrastructure for a given configuration.

### Environment Consistency

Resource naming and tagging conventions are standardized through shared configuration components.

### Modular Infrastructure

Infrastructure is composed using reusable CDK constructs, each responsible for a single area of concern.

Examples include:

- Storage
- Messaging
- Notifications
- Monitoring

### Cost Awareness

Development resources are intentionally designed to support frequent creation and destruction without loss of infrastructure definitions.

---

## Rationale

AWS CDK enables infrastructure to be expressed using the same programming language and engineering practices as the application.

This provides:

- Strong typing
- Compile-time validation
- Code reuse
- Modular design
- Improved maintainability
- Easier onboarding for .NET developers

---

## Consequences

### Benefits

- Infrastructure is reproducible.
- Cloud resources remain synchronized with source control.
- Reduced configuration drift.
- Simplified environment recreation.
- Improved maintainability through reusable constructs.
- Consistent naming and tagging across AWS resources.

### Trade-offs

- Additional learning curve for AWS CDK.
- Longer initial setup compared to manual provisioning.
- Infrastructure changes require deployment rather than console edits.

---

## Alternatives Considered

### Manual AWS Console Configuration

Rejected.

Manual provisioning cannot guarantee repeatability or consistency and increases the likelihood of configuration drift.

### AWS CloudFormation Templates

Rejected.

CloudFormation provides declarative infrastructure but lacks the modularity, abstraction, and programming capabilities offered by AWS CDK.

### Terraform

Considered but not selected.

Terraform is a mature Infrastructure as Code solution; however, AWS CDK was chosen to keep the infrastructure implementation within the .NET ecosystem and leverage shared language features across the application and infrastructure projects.

---

## Implementation Summary

As of **Milestone 4 (v0.4.0)**, the platform provisions infrastructure for:

- Amazon SQS queues
- Amazon DynamoDB tables
- Amazon SNS topics
- CloudWatch dashboards
- CloudWatch alarms
- IAM permissions
- Resource outputs

Resource naming is centralized through:

- `PlatformConfig`
- `ResourceNaming`

Reusable infrastructure is organized into dedicated CDK constructs.

---

## Related ADRs

- ADR-0001 – Project Vision and Architectural Foundation
- ADR-0003 – Asynchronous Event Processing with Amazon SQS
- ADR-0010 – Observability Strategy

---

## Status

This decision remains valid as of **Milestone 4 (v0.4.0)** and continues to govern infrastructure provisioning for the CloudOps Event Platform.