# ADR-0011: Continuous Integration Strategy

**Status:** Accepted

## Context

As the CloudOps Event Platform has grown, manual build verification is no longer sufficient to ensure code quality and maintainability.

Changes should be automatically validated whenever code is pushed to the repository or submitted through a pull request. A Continuous Integration (CI) pipeline reduces integration risk by ensuring the solution builds successfully and passes validation before changes are merged.

The platform currently consists of multiple projects including:

- CloudOps.Api
- CloudOps.Application
- CloudOps.Domain
- CloudOps.Infrastructure
- CloudOps.Worker

Future milestones will introduce automated tests, container builds, and release workflows. The CI strategy should provide a foundation that can evolve without requiring major architectural changes.

## Decision

The platform will adopt **GitHub Actions** as its Continuous Integration platform.

A workflow will execute automatically on pushes and pull requests targeting the main development branch.

The pipeline will perform the following stages:

1. Checkout the repository.
2. Install the required .NET SDK.
3. Restore NuGet packages.
4. Build the solution.
5. Execute automated tests.
6. Verify code formatting.
7. Publish build artifacts when appropriate.

The pipeline will fail immediately if any validation stage fails.

## Consequences

### Positive

- Automated validation of every change.
- Early detection of build failures.
- Consistent build process across contributors.
- Foundation for Continuous Delivery.
- Improved confidence when merging changes.

### Negative

- Slightly longer feedback cycle compared to local builds.
- Initial maintenance overhead for workflow configuration.

## Alternatives Considered

### Manual Validation

Rejected because it is error-prone and does not scale as the project grows.

### Azure DevOps Pipelines

Rejected because GitHub Actions provides native integration with the repository and satisfies the project's current requirements.

## Related ADRs

- ADR-0002 – Infrastructure as Code with AWS CDK
- ADR-0009 – Event Processing Pipeline Architecture