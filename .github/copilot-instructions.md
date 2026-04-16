# Repository Instructions — Employee Management Portal

## Project Context
- .NET 10 ASP.NET Core Web API
- Entity Framework Core with InMemory provider (structured for SQL Server migration)
- Swagger/OpenAPI for API documentation

## Coding Standards
- Use C# 13 features where appropriate (primary constructors, collection expressions)
- Follow Clean Architecture: thin controllers delegate to service layer
- Use `ILogger<T>` for logging — never `Console.WriteLine`
- Use `async/await` for ALL I/O operations (DB, HTTP, file)
- Pass `CancellationToken` to all async methods
- DTOs for API input/output — never expose EF entities directly
- Use `record` types for DTOs (immutable, concise)
- Status values must be `enum`, not magic strings

## Validation
- Use FluentValidation for input validation
- Validate at controller entry point via pipeline
- Return structured `ProblemDetails` for validation errors

## Security
- No hardcoded secrets — use User Secrets (dev) or environment variables (prod)
- No `FromSqlRaw` with string interpolation — use LINQ or `FromSqlInterpolated`
- All endpoints that expose PII (salary, email) require authorization

## Testing
- Test naming: `MethodName_Scenario_ExpectedResult`
- Use xUnit + FluentAssertions + Moq
- Integration tests use `WebApplicationFactory<Program>`
