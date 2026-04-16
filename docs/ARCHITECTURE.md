# Architecture Overview

## Current State (as-is)

The Employee Portal is a monolithic ASP.NET Core Web API with the following structure:

```
┌─────────────────────────────────────────┐
│              API Layer                   │
│  ┌──────────┐ ┌──────────┐ ┌─────────┐ │
│  │ Employee  │ │  Leave   │ │ Report  │ │
│  │Controller │ │Controller│ │Controller│ │
│  └──────┬───┘ └────┬─────┘ └────┬────┘ │
│         │          │             │       │
│         └──────┬───┘─────────────┘       │
│                │                         │
│        ┌───────┴────────┐                │
│        │  EF Core       │                │
│        │  DbContext      │                │
│        └───────┬────────┘                │
│                │                         │
│        ┌───────┴────────┐                │
│        │  InMemory DB   │                │
│        └────────────────┘                │
└─────────────────────────────────────────┘
```

### Known Issues
- No service layer — business logic lives in controllers
- No DTOs — entities exposed directly via API
- No authentication or authorization
- SQL injection vulnerabilities in ReportController
- Hardcoded secrets in appsettings.json
- No test coverage

## Target State (to-be)

```
┌──────────────────────────────────────────────┐
│              API Layer (thin controllers)      │
│  ┌──────────┐ ┌──────────┐ ┌──────────────┐  │
│  │ Employee  │ │  Leave   │ │   Report     │  │
│  │Controller │ │Controller│ │  Controller  │  │
│  └──────┬───┘ └────┬─────┘ └──────┬───────┘  │
│         │          │               │          │
│  ┌──────┴───┐ ┌────┴─────┐ ┌──────┴───────┐  │
│  │ Employee  │ │  Leave   │ │   Report     │  │
│  │ Service   │ │ Service  │ │   Service    │  │
│  └──────┬───┘ └────┬─────┘ └──────┬───────┘  │
│         │          │               │          │
│  ┌──────┴──────────┴───────────────┴───────┐  │
│  │           EF Core DbContext             │  │
│  └─────────────────┬──────────────────────┘  │
│                    │                          │
│  ┌─────────────────┴──────────────────────┐  │
│  │        SQL Server / InMemory            │  │
│  └─────────────────────────────────────────┘  │
└──────────────────────────────────────────────┘
```

### Improvements Needed
- Extract service layer (IEmployeeService, ILeaveService, IReportService)
- Create DTOs (request/response records)
- Add FluentValidation
- Add async/await throughout
- Fix SQL injection → LINQ
- Add ILogger<T> structured logging
- Add authentication (JWT or SPID/CIE)
- Add test project (xUnit + FluentAssertions)
- Move secrets to User Secrets / Key Vault
