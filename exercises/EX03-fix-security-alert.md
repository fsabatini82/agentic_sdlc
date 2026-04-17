# EX03: Fix Security Alert (CodeQL)

## Type: Cloud Agent (github.com)
## Difficulty: Easy
## Duration: 5 min
## Prerequisite: GitHub Advanced Security (GHAS) attivo, oppure solo demo concettuale

---

## Scenario
Assegnare un code scanning alert a Copilot per fix automatico in ~30 secondi.

## Steps (con GHAS)

1. Apri il repo su github.com
2. Tab **Security → Code scanning alerts**
3. Trova l'alert: "SQL injection" su `ReportController.cs`
4. Click sull'alert → **"Generate fix"** oppure **Assignees → Copilot**
5. Copilot apre una draft PR in ~30 secondi con:
   - `FromSqlRaw($"...")` → `_context.Employees.Where(e => e.Department == department)`
   - Spiegazione nel PR body
   - Test di verifica (se configurato)

## Steps (senza GHAS — demo concettuale)

1. Mostra il codice vulnerabile in `ReportController.cs`:
   ```csharp
   var employees = _context.Employees
       .FromSqlRaw($"SELECT * FROM Employees WHERE Department = '{department}'")
       .ToList();
   ```
2. Spiega: "Con GHAS, CodeQL rileva questa vulnerabilita' automaticamente"
3. Mostra il fix atteso: LINQ `.Where()`
4. Mostra che `dotnet build` produce il warning EF1002

## Cosa Osservare
- Velocita': ~30 secondi dalla assegnazione alla PR
- Qualita': il fix usa LINQ parametrizzato, non un altro tipo di raw SQL
- Completezza: entrambe le istanze (GetSalaryReport + GetLeaveReport) fixate?
