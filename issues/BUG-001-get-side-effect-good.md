## Bug: GET /api/employees/{id} modifies AuditTrail on every read

### Description
The `GET /api/employees/{id}` endpoint appends to the `AuditTrail` field
and calls `SaveChanges()`. This violates HTTP GET idempotency — read
operations must NOT modify server state.

### Steps to Reproduce
1. `GET /api/employees/1` — note the `AuditTrail` value
2. `GET /api/employees/1` again — `AuditTrail` now has a new entry
3. Repeat 10 times — `AuditTrail` has grown by 10 entries

### Expected Behavior
`GET` requests must be side-effect-free. Reading an employee record must
not modify any data in the database.

### Actual Behavior
Each `GET /api/employees/{id}` appends `"; Viewed at {timestamp}"` to the
`AuditTrail` field and persists it via `SaveChanges()`.

### Impact
- **Data integrity**: AuditTrail grows unbounded, clutters real audit data
- **Performance**: Unnecessary write + DB round-trip on every single read
- **Compliance**: Audit log should only record real mutations, not reads
- **Caching**: Response can never be cached because it mutates state

### Suggested Fix
Remove the `SaveChanges()` call and the AuditTrail append from the GET
endpoint. If read tracking is needed, use a separate analytics/logging
mechanism (e.g., Application Insights, a read log table, or an event).

### Affected File
`Controllers/EmployeeController.cs` — `GetById()` method
