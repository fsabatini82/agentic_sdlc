# EX01: Feature from User Story

## Type: Cloud Agent (github.com)
## Difficulty: Easy
## Duration: 10 min

---

## Scenario
Assegnare una User Story ben formattata a Copilot e osservare l'intero flow issue → PR.

## Issue da creare nel repo

```markdown
## [US-001] Add pagination to GET /api/employees

### User Story
As a **department manager**, I want the employee list to support pagination
so that the HR dashboard loads quickly even with 10,000+ employees.

### Acceptance Criteria
- [ ] `GET /api/employees` accepts `?page=1&pageSize=20` query parameters
- [ ] Default `pageSize` is 20, maximum allowed is 100
- [ ] Response includes: `items`, `totalCount`, `page`, `pageSize`, `totalPages`
- [ ] `page < 1` or `pageSize < 1` returns `400 Bad Request` with clear message
- [ ] `page > totalPages` returns empty items array (not 404)
- [ ] Existing behavior without query params remains unchanged

### Technical Notes
- Use EF Core `.Skip()` and `.Take()`
- Create a `PaginatedResponse<T>` wrapper DTO
- Add async/await — the current endpoint is synchronous
- Do NOT modify the `Employee` entity

### Out of Scope
- Sorting and filtering (separate story)
- Cursor-based pagination
```

## Steps

1. Crea la issue nel repo `agentic_sdlc`
2. Nella issue: **Assignees → Copilot**
3. Opzionale: seleziona un modello (Auto o Sonnet 4.6)
4. Copilot reagisce con 👀 e lancia un Actions job
5. Attendi 3-8 minuti
6. Osserva la draft PR:
   - Branch creato
   - Codice generato: `PaginatedResponse<T>`, controller aggiornato
   - PR body con summary delle modifiche
   - Self-review eseguito
   - Security scan passato

## Cosa Osservare
- L'agent legge le Acceptance Criteria e le implementa una per una?
- Ha aggiunto async/await come indicato nelle Technical Notes?
- Ha creato il DTO generico come suggerito?
- Il codice segue le `copilot-instructions.md`?
- Il self-review ha trovato problemi?

## Variante: Confronto con Bad Issue
Dopo EX01, eseguire EX02 (bad issue) e confrontare le due PR side by side.
