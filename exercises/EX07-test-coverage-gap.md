# EX07: Test Coverage Gap Analysis

## Type: CLI + GitHub Actions
## Difficulty: Medio
## Duration: 10 min

---

## Scenario
Analizzare le aree del codice senza test coverage e suggerire test specifici.

## Prompt CLI

```bash
copilot -p "Analyze this .NET project. There are currently ZERO tests.
Identify the most critical areas that need test coverage:
1. For each Controller, list the methods that need testing
2. For each method, suggest 2-3 specific test cases with names
3. Prioritize by: security-critical > business-logic > CRUD
4. Use naming convention: MethodName_Scenario_ExpectedResult

Focus on:
- LeaveRequestController.ApproveLeave (complex business rules)
- ReportController.GetSalaryReport (SQL injection — prove it exists)
- EmployeeController.Create (validation gaps)

Write to test-gap-report.md" \
  --allow-tool=read \
  --allow-tool=write \
  --no-ask-user
```

## Risultato Atteso
Report con test suggeriti tipo:
```
## LeaveRequestController
### ApproveLeave
- ApproveLeave_WhenAlreadyApproved_ShouldReturnConflict
- ApproveLeave_WhenInsufficientDays_ShouldReturnBadRequest
- ApproveLeave_WhenDateConflict_ShouldReturnBadRequest
- ApproveLeave_WhenValid_ShouldUpdateStatusAndNotify

## ReportController
### GetSalaryReport
- GetSalaryReport_WithSqlInjectionPayload_ShouldNotExposeData
- GetSalaryReport_WithValidDepartment_ShouldReturnFilteredResults
```
