# ADR-0012: Containerization Strategy

- **Status:** Accepted
- **Date:** August 2026
- **Decision Makers:** CloudOps Platform Team

---

# Context

CloudOps Event Platform requires a consistent execution environment across developer workstations, CI pipelines, and future deployment environments.

Running applications directly from the SDK introduces inconsistencies in runtime configuration and dependency management.

Containerization provides a reproducible execution environment and establishes the foundation for future Kubernetes and ECS deployments.

---

# Decision

Both deployable services shall be distributed as OCI-compliant Docker images.

Separate production Dockerfiles are maintained for:

- CloudOps.Api
- CloudOps.Worker

Each image is built using multi-stage Docker builds to:

- reduce image size
- separate build and runtime concerns
- improve security
- improve build reproducibility

Docker Compose is provided for local multi-service development.

---

# Consequences

## Positive

- Consistent runtime environments
- Faster onboarding
- Deployment portability
- CI/CD compatibility
- Foundation for orchestration platforms

## Negative

- Additional maintenance for Dockerfiles
- Local Docker dependency
- AWS credentials required for Worker execution

---

# Alternatives Considered

## SDK-only execution

Rejected because deployment environments should not require the .NET SDK.

## Single Docker image

Rejected because API and Worker have different runtime responsibilities and scaling characteristics.

---

# References

- docs/runbooks/docker.md