## [SEC-001] SQL Injection in ReportController.GetSalaryReport

### Severity: CRITICAL

### Description
The `GET /api/reports/salary?department=X` endpoint uses `FromSqlRaw()`
with C# string interpolation, making it vulnerable to SQL injection attacks.
An attacker can read, modify, or delete any data in the database.

### Vulnerable Code
```csharp
// Controllers/ReportController.cs, GetSalaryReport method
var employees = _context.Employees
    .FromSqlRaw($"SELECT * FROM Employees WHERE Department = '{department}'")
    .ToList();
```

### Proof of Concept
```http
GET /api/reports/salary?department='; DROP TABLE Employees; --
```
This would execute:
```sql
SELECT * FROM Employees WHERE Department = ''; DROP TABLE Employees; --'
```

### A Second Instance
The same vulnerability exists in `GetLeaveReport`:
```csharp
// Same file, GetLeaveReport method
var requests = _context.LeaveRequests
    .FromSqlRaw($"SELECT * FROM LeaveRequests WHERE Status = '{status}'")
    .ToList();
```

### Acceptance Criteria
- [ ] No `FromSqlRaw` with string interpolation anywhere in the codebase
- [ ] Replace with LINQ `.Where()` (preferred) or `FromSqlInterpolated()`
- [ ] Both `GetSalaryReport` and `GetLeaveReport` are fixed
- [ ] Endpoints return identical results with safe queries
- [ ] Add a unit test that verifies parameterized input (no injection possible)

### References
- [OWASP SQL Injection](https://owasp.org/www-community/attacks/SQL_Injection)
- [EF Core Raw SQL](https://learn.microsoft.com/en-us/ef/core/querying/raw-sql)
