# EX08: Auto-Triage Issues (Agentic Workflow)

## Type: Agentic Workflow (gh aw)
## Difficulty: Medio
## Duration: 15 min

---

## Scenario
Workflow scritto in Markdown (non YAML) che classifica e labella automaticamente le issue appena aperte.

## Workflow File

Gia' presente in `.github/workflows/auto-triage.md`:

```markdown
# Auto-Triage Issues

When a new issue is opened in this repository:
1. Read the issue title and body carefully
2. Classify as: bug | feature | question | docs | security
3. Apply the appropriate type label
4. Assess priority: high | medium | low
5. If security: apply "security" label, add escalation comment
6. If bug with stack trace: search codebase for related files, comment
7. If question answered in docs: reply with answer, suggest closing
```

## Steps

1. Installare gh aw (se non installato):
   ```bash
   gh extension install github/gh-aw
   ```
2. Sincronizzare il workflow:
   ```bash
   gh aw sync
   ```
3. Creare una issue di test:
   - Issue tipo bug: "Error 500 when approving leave request with negative days"
   - Issue tipo feature: "Add email notification when leave is approved"
   - Issue tipo bad: "fix the leave thing" (dalla bad issue)
4. Osservare le label applicate e i commenti generati

## Trigger Supportati da Agentic Workflows

| Trigger | Descrizione | Esempio |
|---------|-------------|---------|
| `issues` | Issue aperta/editata/labellata | `on: issue opened` |
| `pull_request` | PR aperta/sincronizzata | `on: pull_request` |
| `issue_comment` | Commento su issue/PR | `on: issue comment` |
| `schedule` | Cron o linguaggio naturale | `on: daily`, `cron: '0 6 * * 1'` |
| `workflow_dispatch` | Lancio manuale | `on: workflow_dispatch` |
| `push` | Commit su branch/tag | `on: push to main` |
| `release` | Release pubblicata | `on: release published` |
| `slash_command` | Comando `/nome` in commento | `on: /triage` |
| `label_command` | Label applicata (auto-rimossa) | `on: label "needs-triage"` |
| `workflow_run` | Dopo un altro workflow | `on: workflow_run` |
| `pull_request_review_comment` | Commento di review | |
| `discussion_comment` | Commento su discussion | |
| `watch` | Repo starred | |
| `fork` | Repo forkato | |
| `repository_dispatch` | API custom event | |
