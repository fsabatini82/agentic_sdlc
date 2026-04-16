## [US-001] Add pagination to GET /api/employees

### User Story
As a **department manager**, I want the employee list to support pagination
so that the HR dashboard loads quickly even with 10,000+ employees.

### Acceptance Criteria
- [ ] `GET /api/employees` accepts `?page=1&pageSize=20` query parameters
- [ ] Default `pageSize` is 20, maximum allowed is 100
- [ ] Response body includes: `items`, `totalCount`, `page`, `pageSize`, `totalPages`
- [ ] `page < 1` or `pageSize < 1` returns `400 Bad Request` with clear error message
- [ ] `page > totalPages` returns empty `items` array (not 404)
- [ ] Existing behavior without query params remains unchanged (returns first page)

### Technical Notes
- Use EF Core `.Skip()` and `.Take()` for efficient DB-level pagination
- Create a generic `PaginatedResponse<T>` wrapper DTO
- Add `async/await` — the current endpoint is synchronous
- Do NOT modify the `Employee` entity

### Out of Scope
- Sorting and filtering (separate story [US-002])
- Cursor-based pagination (future consideration)
- UI changes (handled by frontend team)
