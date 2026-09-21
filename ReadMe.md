# IssueTracker API

I built this small ASP.NET Core Web API for managing issues as a hands-on backend project going beyond basic CRUD by separating responsibilities, handling failures and concurrency, testing against a real database, securing an API, and getting it running in CI and Docker.

The project is intentionally small. The goal was to learn solid backend near production-grade and the trade-offs behind them, rather than demonstrate as many technologies or architectural patterns as possible.

## What it demonstrates

* ASP.NET Core Web API
* Layered architecture
* Domain-driven business rules
* Separate HTTP and application contracts
* EF Core + SQL Server
* Repository + Unit of Work
* Result Pattern
* Global exception handling + ProblemDetails
* Optimistic concurrency with SQL Server `rowversion`
* JWT authentication and scope-based authorization
* Health checks and structured logging
* Unit and integration testing with MSTest and Moq
* Testcontainers for SQL Server integration tests
* Docker + Docker Compose
* EF Core migrations in Docker
* GitHub Actions CI

## Architecture and separation

The API is intentionally a layered monolith:

```text
HTTP / Controllers
        ↓
Application / Services
        ↓
Domain
        ↓
Infrastructure / EF Core / SQL Server
```

The main goal was to keep each boundary understandable. HTTP request/response models are separate from application inputs and outputs, while business rules stay inside the domain model rather than leaking into controllers.

I deliberately avoided adding patterns such as CQRS, messaging, or microservices where the current domain doesn't benefit from them.

## Persistence

EF Core and SQL Server handle persistence behind a small repository and Unit of Work boundary.

The application service works with domain objects and repositories rather than directly with EF Core. Database changes are committed through the Unit of Work, giving the application an explicit persistence boundary.

Optimistic concurrency is handled with SQL Server `rowversion`. When EF Core detects that an entity has been changed since it was read, the application translates the concurrency exception into a conflict result and the API returns HTTP 409.

## Error handling and reliability

Expected application failures are represented with the Result Pattern rather than exceptions.

Unexpected exceptions are handled centrally through ASP.NET Core's exception handling pipeline and returned as `ProblemDetails`.

Structured logging is used for unexpected exceptions and concurrency conflicts, while a health-check endpoint verifies database connectivity.

## Testing

The project contains both unit and integration tests.

Unit tests cover application behaviour in isolation. Integration tests use a real SQL Server instance through Testcontainers, allowing EF Core and database-specific behaviour to be tested rather than mocked away.

HTTP integration tests also cover the authentication and authorization boundary.

Some parts of the project were developed test-first as a way to explore behaviour and design decisions, rather than treating tests only as regression coverage.

## Authentication

The API uses JWT bearer authentication with scope-based authorization policies for reading, writing, and deleting issues.

For local development, `dotnet-user-jwts` is used to generate tokens. The project does not attempt to implement its own identity provider.

## CI and Docker

GitHub Actions restores, builds, and tests the solution in a clean environment.

Docker Compose runs the API and SQL Server together, with a separate migration step applying the EF Core migrations before the API is used.

The database credentials used by Docker are supplied through `.env` and are not committed to the repository.
