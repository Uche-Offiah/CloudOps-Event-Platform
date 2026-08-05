# ADR-0011: Continuous Integration Strategy

- **Status:** Accepted
- **Date:** August 2026
- **Decision Makers:** CloudOps Platform Team

---

# Context

The project requires automated validation to ensure code quality before changes reach protected branches.

Manual validation is error-prone and inconsistent.

---

# Decision

GitHub Actions is adopted as the project's Continuous Integration platform.

Each workflow execution performs:

1. Restore
2. Build
3. Code formatting verification
4. Unit tests
5. Integration tests
6. Publish API artifacts
7. Publish Worker artifacts

Artifacts are retained to support future deployment workflows.

---

# Consequences

## Positive

- Automated validation
- Consistent builds
- Early defect detection
- Release-ready artifacts
- Reduced manual verification

## Negative

- Longer build times
- GitHub Actions dependency

---

# Future Enhancements

- NuGet package caching
- Container image publishing
- Security scanning
- SBOM generation
- Release automation

---

# References

.github/workflows/ci.yml