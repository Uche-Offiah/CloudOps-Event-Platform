# CloudOps Event Platform

> A production-grade, event-driven cloud platform built on AWS using .NET and Infrastructure as Code.

## Overview

CloudOps Event Platform is a portfolio project designed to simulate an internal operations platform used by cloud engineering and DevOps teams.

The platform ingests operational events, processes them asynchronously, stores historical records, generates notifications, and exposes APIs for querying and monitoring system activity.

Unlike a typical CRUD application, this project focuses on cloud-native architecture, distributed systems, observability, and engineering best practices.

The primary goal is to demonstrate the design and implementation of a production-ready cloud platform using modern AWS services and .NET.

---

## Project Goals

* Build an event-driven cloud-native application.
* Apply production-level software engineering principles.
* Provision all infrastructure using AWS CDK.
* Implement automated deployment through GitHub Actions.
* Demonstrate observability, resiliency, and operational excellence.
* Create a portfolio project representative of real-world engineering practices.

---

## Planned Architecture

```text
                Client
                   │
                   ▼
             API Gateway
                   │
                   ▼
          ASP.NET Core API
                   │
             Validation Layer
                   │
                   ▼
              Amazon SQS
                   │
        ┌──────────┴──────────┐
        ▼                     ▼
   Worker Service      Dead Letter Queue
        │
        ▼
     DynamoDB
        │
        ├─────────────┐
        ▼             ▼
 CloudWatch         SNS
 Metrics        Notifications
```

---

## Technology Stack

### Backend

* .NET 10
* ASP.NET Core Minimal APIs
* C#
* Dependency Injection
* MediatR (planned)
* FluentValidation (planned)
* Serilog (planned)

### AWS

* API Gateway
* AWS Lambda
* Amazon SQS
* Amazon SNS
* Amazon DynamoDB
* Amazon CloudWatch
* AWS IAM
* AWS Systems Manager Parameter Store
* AWS Secrets Manager

### Infrastructure

* AWS CDK (C#)
* Infrastructure as Code (IaC)

### Testing

* xUnit
* Integration Tests
* Architecture Tests

### CI/CD

* GitHub Actions (planned)

---

## Repository Structure

```text
CloudOps-Event-Platform/

├── src/
├── tests/
├── infra/
├── docs/
│   ├── architecture/
│   ├── adr/
│   ├── diagrams/
│   └── runbooks/
├── scripts/
└── .github/
```

---

## Engineering Principles

This project emphasizes engineering quality over feature count.

Key principles include:

* Clean Architecture
* SOLID Principles
* Separation of Concerns
* Event-Driven Architecture
* Infrastructure as Code
* Least-Privilege Security
* Automated Testing
* Continuous Integration
* Observability
* Operational Readiness

---

## Current Status

**Project Phase:** Milestone 0 — Foundation

Current work includes:

* Repository initialization
* Solution structure
* Project organization
* Infrastructure setup with AWS CDK
* Architecture documentation
* Engineering standards

---

## Roadmap

* ✅ Repository foundation
* ⏳ Infrastructure as Code
* ⏳ Event API
* ⏳ Worker Service
* ⏳ Notifications
* ⏳ Observability
* ⏳ Authentication
* ⏳ CI/CD
* ⏳ Production hardening

---

## Learning Objectives

This project is intended to strengthen practical experience in:

* Cloud Engineering
* AWS Architecture
* Distributed Systems
* Backend Development
* Infrastructure Automation
* DevOps Practices
* Software Architecture
* Production Operations

---

## License

This project is licensed under the MIT License.

