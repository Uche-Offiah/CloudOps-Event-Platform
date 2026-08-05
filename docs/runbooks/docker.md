# Docker Runbook

- **Document ID:** RB-DOCKER-001
- **Status:** Active
- **Owner:** Platform Engineering
- **Last Updated:** August 2026

---

# 1. Purpose

This runbook provides operational guidance for building, running, and troubleshooting the CloudOps Event Platform using Docker.

It is intended for developers, DevOps engineers, and platform maintainers who need to execute the application in a containerized environment for local development, validation, or continuous integration.

This runbook covers:

- Building Docker images
- Running the API and Worker containers
- Using Docker Compose
- AWS credential requirements
- Common troubleshooting scenarios

---

# 2. Scope

This runbook applies to the following services:

| Service | Dockerfile |
|----------|------------|
| CloudOps API | `src/CloudOps.Api/Dockerfile` |
| CloudOps Worker | `src/CloudOps.Worker/Dockerfile` |

---

# 3. Prerequisites

Ensure the following software is installed.

| Software | Version |
|----------|---------|
| Docker Desktop / Docker Engine | Latest |
| .NET SDK | 10.0 (optional for local development) |
| Git | Latest |
| AWS CLI | Version 2 |

Verify Docker is installed.

```bash
docker --version
```

Verify Docker Compose.

```bash
docker compose version
```

Verify AWS CLI.

```bash
aws --version
```

---

# 4. Repository Structure

The repository contains dedicated Dockerfiles for each deployable service.

```text
src/
├── CloudOps.Api/
│   └── Dockerfile
│
└── CloudOps.Worker/
    └── Dockerfile

docker-compose.yml
```

---

# 5. Building Images

## Build API

From the repository root:

```bash
docker build \
    -f src/CloudOps.Api/Dockerfile \
    -t cloudops-api:latest .
```

Expected result:

```text
Successfully tagged cloudops-api:latest
```

---

## Build Worker

```bash
docker build \
    -f src/CloudOps.Worker/Dockerfile \
    -t cloudops-worker:latest .
```

Expected result:

```text
Successfully tagged cloudops-worker:latest
```

---

# 6. Verify Images

List locally available images.

```bash
docker images
```

Expected output includes:

```text
cloudops-api
cloudops-worker
```

---

# 7. Running the API

Start the API container.

```bash
docker run \
    --rm \
    -p 8080:8080 \
    cloudops-api:latest
```

Verify the Health endpoint.

```http
GET http://localhost:8080/health
```

Expected response:

```text
HTTP 200 OK
```

---

# 8. Running the Worker

The Worker communicates directly with AWS services.

Unlike the API, it requires valid AWS credentials before startup.

## Using Environment Variables

```bash
docker run \
    --rm \
    -e AWS_ACCESS_KEY_ID=<access-key> \
    -e AWS_SECRET_ACCESS_KEY=<secret-key> \
    -e AWS_REGION=us-east-2 \
    cloudops-worker:latest
```

---

## Using AWS CLI Credentials (Recommended)

Mount the local AWS configuration directory.

Linux/macOS

```bash
docker run \
    --rm \
    -v ~/.aws:/root/.aws:ro \
    -e AWS_PROFILE=default \
    -e AWS_REGION=us-east-2 \
    cloudops-worker:latest
```

Windows PowerShell

```powershell
docker run `
    --rm `
    -v "${env:USERPROFILE}\.aws:/root/.aws:ro" `
    -e AWS_PROFILE=default `
    -e AWS_REGION=us-east-2 `
    cloudops-worker:latest
```

> **Note**
>
> In production deployments, AWS credentials should be supplied using an IAM Role rather than static credentials.

---

# 9. Running with Docker Compose

Build and start all services.

```bash
docker compose up --build
```

Run in detached mode.

```bash
docker compose up -d
```

Stop services.

```bash
docker compose down
```

View logs.

```bash
docker compose logs
```

View logs for a single service.

```bash
docker compose logs api

docker compose logs worker
```
# Adding docker compose to amazon linux ec2 instance

# Create the global CLI plugins directory
sudo mkdir -p /usr/libexec/docker/cli-plugins/

# Download the latest stable Docker Compose release binary
sudo curl -SL https://github.com/docker/compose/releases/latest/download/docker-compose-linux-$(uname -m) -o /usr/libexec/docker/cli-plugins/docker-compose

# Grant execute permissions to the binary 
sudo chmod +x /usr/libexec/docker/cli-plugins/docker-compose

---

# 10. Troubleshooting

## Docker Build Fails

Possible causes:

- Missing Docker installation
- Incorrect Dockerfile path
- Build context is incorrect
- NuGet restore failure

Recommended actions:

- Verify Docker is running.
- Confirm the build command is executed from the repository root.
- Inspect build output for the failing layer.

---

## API Does Not Start

Symptoms:

- Container exits immediately.
- Port 8080 is unavailable.

Resolution:

Verify another process is not already using port 8080.

```bash
docker ps

netstat -ano
```

---

## Worker Fails to Start

Example:

```text
AmazonClientException:
Failed to resolve AWS credentials.
```

Possible causes:

- AWS credentials are missing.
- AWS CLI profile does not exist.
- Credentials have expired.
- AWS region is not configured.

Resolution:

- Configure AWS CLI credentials.
- Supply environment variables.
- Mount the AWS credentials directory.
- Verify the configured AWS profile.

---

## Verify AWS Credentials

Confirm AWS credentials are available.

```bash
aws sts get-caller-identity
```

Expected output:

```json
{
  "Account": "...",
  "Arn": "...",
  "UserId": "..."
}
```

---

## View Container Logs

```bash
docker logs <container-id>
```

---

## Remove Stopped Containers

```bash
docker container prune
```

---

## Remove Unused Images

```bash
docker image prune
```

---

# 11. Security Considerations

- Never bake AWS credentials into Docker images.
- Never commit AWS credentials to source control.
- Prefer AWS CLI profiles for local development.
- Prefer IAM Roles for production deployments.
- Use read-only credential mounts where possible.

---

# 12. Future Enhancements

Planned improvements include:

- LocalStack integration
- Offline local development
- Automated bootstrap scripts
- Container health checks
- Container vulnerability scanning
- Image signing
- Multi-architecture image builds

---

# 13. References

- `README.md`
- `docker-compose.yml`
- `src/CloudOps.Api/Dockerfile`
- `src/CloudOps.Worker/Dockerfile`
- `docs/architecture/observability.md`
- AWS SDK for .NET documentation
- Docker documentation

---

# Revision History

| Version | Date | Description |
|----------|------|-------------|
| 1.0 | August 2026 | Initial Docker runbook for CloudOps Event Platform. |