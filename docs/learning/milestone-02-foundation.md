# Milestone 02 Retrospective — Application Foundation

## Overview

This milestone established the application architecture for the CloudOps Event Platform.

The goal was not to implement business functionality but to create a foundation that supports future development without requiring major structural changes.

---

# What Was Built

Created a .NET solution containing:

* CloudOps.Api
* CloudOps.Application
* CloudOps.Domain
* CloudOps.Infrastructure
* CloudOps.Contracts
* CloudOps.SharedKernel

Added:

* Unit test project.
* Integration test project.
* Shared build configuration.
* Nullable reference types.
* Warning-as-error enforcement.

---

# Key Concepts Learned

## Clean Architecture

The application was separated into layers with clear responsibilities.

Business logic remains independent from infrastructure and external systems.

---

## Dependency Direction

Dependencies point inward toward business logic.

The domain does not depend on AWS, databases, or APIs.

---

## Vertical Slice Architecture

Features will be organized around user capabilities rather than technical categories.

This improves maintainability as the application grows.

---

# Engineering Decisions

## Why multiple projects?

Separating responsibilities creates stronger boundaries and prevents accidental coupling.

---

## Why not start with the API?

Starting with endpoints often leads to business logic being placed inside controllers.

The project establishes architecture before functionality.

---

# Challenges

The primary challenge was balancing structure with simplicity.

The architecture intentionally provides enough organization for growth without introducing unnecessary complexity.

---

# Next Steps

Milestone 2.1 will introduce the first complete vertical slice:

Event Submission.

The flow will be:

```
HTTP Request

↓

Validation

↓

Application Command

↓

Publish Message

↓

SQS
```

This will connect the application layer with the AWS infrastructure created during Milestone 1.
