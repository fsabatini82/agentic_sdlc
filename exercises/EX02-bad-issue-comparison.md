# EX02: Bad Issue — Garbage In, Garbage Out

## Type: Cloud Agent (github.com)
## Difficulty: Easy
## Duration: 5 min (+ confronto con EX01)

---

## Scenario
Mostrare l'impatto di una issue mal formattata sul risultato dell'agent.

## Issue da creare nel repo

```markdown
## fix the leave thing

it doesn't work when i try to do stuff with the leaves.
sometimes it says error and sometimes it just does nothing.
also the dates are weird.

can someone fix this? its urgent
```

## Steps

1. Crea la issue nel repo
2. **Assignees → Copilot**
3. Attendi la PR
4. Osserva il risultato — sara' vago, generico, probabilmente sbagliato

## Confronto Side by Side

| Aspetto | Good Issue (EX01) | Bad Issue (EX02) |
|---------|-------------------|------------------|
| Acceptance Criteria | 6 criteri checkable | Nessuno |
| Fix specifico | PaginatedResponse<T> | Fix generico/randomico |
| Test inclusi | Si, specifici | No o generici |
| PR body | Summary strutturato | Vago |
| Tempo agent | ~5 min | ~5 min (stesso tempo, risultato peggiore) |

## Punto Chiave
> "Garbage in, garbage out. L'agent spende lo stesso tempo, ma il risultato e' radicalmente diverso. La qualita' della specifica determina la qualita' del codice."
