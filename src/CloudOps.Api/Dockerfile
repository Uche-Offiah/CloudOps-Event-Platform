# ============================================================
# Build Stage
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy solution
COPY CloudOps.slnx ./

# Copy project files
COPY src/CloudOps.Api/CloudOps.Api.csproj src/CloudOps.Api/
COPY src/CloudOps.Application/CloudOps.Application.csproj src/CloudOps.Application/
COPY src/CloudOps.Contracts/CloudOps.Contracts.csproj src/CloudOps.Contracts/
COPY src/CloudOps.Domain/CloudOps.Domain.csproj src/CloudOps.Domain/
COPY src/CloudOps.Infrastructure/CloudOps.Infrastructure.csproj src/CloudOps.Infrastructure/
COPY src/CloudOps.Shared/CloudOps.Shared.csproj src/CloudOps.Shared/
COPY src/CloudOps.Worker/CloudOps.Worker.csproj src/CloudOps.Worker/

COPY tests/CloudOps.ArchitectureTests/CloudOps.ArchitectureTests.csproj tests/CloudOps.ArchitectureTests/
COPY tests/CloudOps.IntegrationTests/CloudOps.IntegrationTests.csproj tests/CloudOps.IntegrationTests/
COPY tests/CloudOps.UnitTests/CloudOps.UnitTests.csproj tests/CloudOps.UnitTests/

# Restore packages
RUN dotnet restore CloudOps.slnx

# Copy remaining source
COPY . .

# Publish API
RUN dotnet publish src/CloudOps.Api/CloudOps.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ============================================================
# Runtime Stage
# ============================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CloudOps.Api.dll"]