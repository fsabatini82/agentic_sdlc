# EX10: Dependency Update + Migration

## Type: Cloud Agent (github.com)
## Difficulty: Difficile
## Duration: 15 min

---

## Scenario
Migrazione da una libreria legacy a una nativa .NET, gestita interamente dall'agent.

## Issue

```markdown
## [US-010] Migrate from Newtonsoft.Json to System.Text.Json

### Context
The project currently uses Newtonsoft.Json for JSON serialization.
.NET 10 has mature System.Text.Json support. We should migrate
to reduce dependencies and improve performance.

### Acceptance Criteria
- [ ] Remove Newtonsoft.Json NuGet package
- [ ] Replace all `JsonConvert.SerializeObject` with `JsonSerializer.Serialize`
- [ ] Replace all `JsonConvert.DeserializeObject<T>` with `JsonSerializer.Deserialize<T>`
- [ ] Handle naming policy differences (camelCase by default in STJ)
- [ ] Handle null handling differences
- [ ] All existing API responses remain unchanged
- [ ] `dotnet build` succeeds with zero warnings related to JSON

### Technical Notes
- STJ uses `JsonSerializerOptions` instead of `JsonSerializerSettings`
- Property naming: use `JsonSerializerDefaults.Web` for camelCase
- DateTime handling may differ — verify serialization format

### Out of Scope
- Custom JSON converters (if any exist, mark with TODO)
```

## Steps
1. Crea la issue
2. Assignees → Copilot (raccomandato: **Opus 4.6** per task complesso)
3. Osserva la PR: search & replace sistematico + config update + test

## Modello Raccomandato
**Opus 4.6** o **GPT-5.4** — serve conoscenza approfondita delle differenze tra le due librerie.
