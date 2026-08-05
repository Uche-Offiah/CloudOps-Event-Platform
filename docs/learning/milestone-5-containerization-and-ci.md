# Milestone 5 Learning

## Overview

Milestone 5 introduced containerization and continuous integration, moving the project toward production readiness.

---

# Key Learnings

## Docker

Separate Dockerfiles for the API and Worker provide independent deployment units while maintaining shared source code.

Multi-stage builds significantly reduce image size and improve security.

---

## Continuous Integration

Automated validation catches issues before merge.

One early pipeline execution failed due to formatting inconsistencies, demonstrating the value of automated code formatting enforcement.

---

## Build Artifacts

Using published application outputs rather than intermediate build outputs produces deployment-ready artifacts suitable for future release pipelines.

---

## AWS Credentials

The Worker requires AWS credentials during startup.

This behavior is expected and documented within the Docker runbook.

Future milestones may introduce LocalStack support for offline development.

---

## Outcome

Milestone 5 established:

- reproducible builds
- automated validation
- deployment-ready artifacts
- operational Docker documentation

These capabilities form the foundation for future Continuous Delivery.