# Employee Management Portal

A .NET 10 ASP.NET Core Web API for managing employees, departments, leave requests, and skills.

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Runtime | .NET 10 |
| Framework | ASP.NET Core Web API |
| ORM | Entity Framework Core (InMemory) |
| API Docs | Swagger / OpenAPI |

## Getting Started

```bash
# Clone the repo
git clone https://github.com/YOUR_ORG/agentic_sdlc.git
cd agentic_sdlc/src/EmployeePortal

# Restore and run
dotnet restore
dotnet run

# Open Swagger UI
# http://localhost:5000/swagger
```

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | /api/employees | List all employees |
| GET | /api/employees/{id} | Get employee by ID |
| POST | /api/employees | Create new employee |
| PUT | /api/employees/{id} | Update employee |
| DELETE | /api/employees/{id} | Delete employee |
| GET | /api/employees/department/{dept} | Search by department |
| GET | /api/departments | List all departments |
| POST | /api/departments | Create department |
| GET | /api/leave-requests | List all leave requests |
| POST | /api/leave-requests | Create leave request |
| POST | /api/leave-requests/{id}/approve | Approve leave |
| POST | /api/leave-requests/{id}/reject | Reject leave |
| GET | /api/leave-requests/balance/{employeeId} | Get leave balance |
| GET | /api/reports/salary | Salary report by department |
| GET | /api/reports/headcount | Headcount report |
| GET | /api/reports/leave | Leave report by status |

## Project Structure

```
src/EmployeePortal/
├── Controllers/
│   ├── EmployeeController.cs
│   ├── DepartmentController.cs
│   ├── LeaveRequestController.cs
│   └── ReportController.cs
├── Models/
│   ├── Employee.cs
│   ├── Department.cs
│   ├── LeaveRequest.cs
│   └── Skill.cs
├── Data/
│   ├── PortalDbContext.cs
│   └── SeedData.cs
├── Program.cs
└── appsettings.json
```

## GitHub Copilot Configuration

This repository includes custom GitHub Copilot agents, skills, and workflows:

- **Agents**: `.github/agents/` — code-reviewer, security-scanner, refactoring-advisor, doc-writer
- **Skills**: `.github/skills/doc-generator/` — branded documentation generator
- **Prompts**: `.github/prompts/` — orchestration prompts
- **Workflows**: `.github/workflows/` — security scan, code quality, auto-triage

## License

For training purposes only.
