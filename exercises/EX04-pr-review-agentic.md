# EX04: PR Review Agentico

## Type: Code Review (github.com)
## Difficulty: Easy
## Duration: 5 min

---

## Scenario
Copilot Code Review su una PR aperta — non il classico "LGTM" ma suggerimenti concreti con codice.

## Steps

1. Apri una PR nel repo (qualsiasi — quella generata in EX01, oppure creane una manuale)
2. Nella PR, click **Copilot → Review** (o aggiungere Copilot come reviewer)
3. Osserva i commenti:
   - Suggerimenti specifici con codice diff
   - Severity per ogni finding
   - "Apply suggestion" con un click
   - Se il finding e' fixabile, puo' generare una fix PR

## Cosa Osservare
- I suggerimenti sono concreti (con codice), non vaghi
- Copilot capisce il contesto del progetto (legge `copilot-instructions.md`)
- Puo' delegare il fix al coding agent con un click

## Variante
Creare una PR intenzionalmente problematica:
```csharp
// PR con codice problematico da revieware
public ActionResult GetEmployees() {
    Console.WriteLine("Getting employees");  // Should be ILogger
    var emps = _context.Employees.ToList();  // No async!
    return Ok(emps);                          // No DTO!
}
```
