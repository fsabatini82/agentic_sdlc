# EX12: Security Campaign Blitz

## Type: Cloud Agent (github.com) — Batch
## Difficulty: Avanzato
## Duration: 15 min
## Prerequisite: GitHub Advanced Security (GHAS)

---

## Scenario
Fix in batch di multipli alert di sicurezza — Copilot apre una PR per ognuno in ~30 secondi.

## Steps

1. Apri tab **Security → Code scanning alerts**
2. Seleziona 3-5 alert (SQL injection, hardcoded secrets, etc.)
3. Per ogni alert: **"Generate fix"** o **Assignees → Copilot**
4. Copilot apre draft PR per ciascuno
5. Review rapida di ogni PR: Approve o Request Changes

## Cosa Osservare
- Velocita': ~30 secondi per fix
- Qualita': ogni fix e' specifico per l'alert
- Batch: piu' PR in parallelo
- Conflitti: se due fix toccano lo stesso file, merge conflict da gestire

## Uso Pratico
> "Security campaign: il CISO chiede di fixare 50 alert entro venerdi'. Con Copilot: 50 draft PR in un'ora. Review umana per ognuna. Merge in giornata."
