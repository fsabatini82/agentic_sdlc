# EX06: Code Smell Detection + Refactoring Proposal

## Type: CLI + GitHub Actions (scheduled)
## Difficulty: Medio
## Duration: 10 min

---

## Scenario
Workflow schedulato (ogni lunedi') che analizza il codebase per code smells e produce un report con proposte di refactoring.

## Workflow File

Gia' presente in `.github/workflows/code-quality.yml`:

```yaml
name: Code Quality Analysis
on:
  workflow_dispatch:
  schedule:
    - cron: '0 6 * * 1'  # Ogni Lunedi 6:00 UTC
```

## Steps

1. Apri tab **Actions** → **"Code Quality Analysis"** → **Run workflow**
2. Attendi 2-3 minuti
3. Leggi lo Step Summary

## Risultato Atteso
- God class: `LeaveRequestController.cs` (276 righe)
- Logica duplicata: overdue/conflict check in 3+ metodi
- Console.WriteLine in tutti i controller (tranne Department)
- Thread.Sleep in LeaveRequest e Employee controller
- Sync calls (`.ToList()` senza async)
- Magic strings ("PENDING", "APPROVED", "TODO")

## Uso Pratico
> "Ogni lunedi' mattina il team riceve un report automatico con i debiti tecnici piu' impattanti. Prioritizzato. Con suggerimenti di fix. Zero effort manuale."
