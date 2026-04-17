# GitHub Copilot (Enterprise) — Agentic Workflow Triggers Reference

> Lista completa degli eventi e entry point che possono attivare una sessione del **Copilot Cloud Agent** (ex "Coding Agent") in GitHub Enterprise.
> Il modello AI sottostante viene scelto in base al workflow di lavoro.

---

## Panoramica

Copilot Cloud Agent lavora **autonomamente** in un ambiente ephemeral (powered by GitHub Actions) per completare task di sviluppo. Ogni trigger avvia una **sessione agent** che produce branch, commit, e opzionalmente una Pull Request.

---

## Trigger Completi — Repo Events & Entry Points

### 1. Issue Assignment

| Trigger | Dettaglio |
|---|---|
| **Assign issue a Copilot** | Assegnare `copilot-swe-agent[bot]` come assignee di una issue |
| **Dove** | GitHub.com UI, GitHub Mobile, GraphQL API, REST API, GitHub CLI (`gh issue edit`), Raycast |
| **Payload opzionale** | `target_repo`, `base_branch`, `custom_instructions`, `custom_agent`, `model` |

```bash
# CLI
gh issue edit 42 --add-assignee copilot-swe-agent[bot]

# REST API
POST /repos/OWNER/REPO/issues/ISSUE_NUMBER/assignees
{
  "assignees": ["copilot-swe-agent[bot]"],
  "agent_assignment": {
    "target_repo": "OWNER/REPO",
    "base_branch": "main",
    "custom_instructions": "...",
    "custom_agent": "",
    "model": ""
  }
}
```

---

### 2. `@copilot` Mention in Pull Request Comment

| Trigger | Dettaglio |
|---|---|
| **Mention `@copilot`** | In un commento su una PR esistente |
| **Effetto** | Copilot apporta modifiche direttamente al branch della PR |
| **Variante** | Chiedere di aprire una nuova PR separata nel commento |
| **Merge conflicts** | `@copilot resolve the merge conflicts on this PR` |
| **Fix with Copilot button** | Bottone nella merge box quando ci sono conflitti |

---

### 3. Agents Panel / Tab (github.com)

| Trigger | Dettaglio |
|---|---|
| **Prompt diretto** | Dalla tab Agents (`github.com/copilot/agents`) o dal pannello Agents |
| **Opzioni** | Selezione repo, base branch, custom agent, modello AI |
| **Supporta immagini** | PNG, JPEG, GIF, WEBP come input visivo |

---

### 4. Dashboard Prompt

| Trigger | Dettaglio |
|---|---|
| **Task button** | Dal dashboard personale (`github.com`) cliccando il bottone Task |
| **Effetto** | Nuova sessione agent → PR |

---

### 5. Copilot Chat — `/task` command (github.com)

| Trigger | Dettaglio |
|---|---|
| **`/task` in Chat** | Da Copilot Chat su github.com con prefisso `/task` |
| **Effetto** | Avvia sessione cloud agent con prompt → PR |

---

### 6. IDE — Delegate to Cloud Agent

| IDE | Trigger |
|---|---|
| **VS Code** | Bottone "Delegate this task to the GitHub Copilot cloud agent" |
| **JetBrains** | Bottone "Delegate to Cloud Agent" |
| **Eclipse** | Icona Agents accanto a Send |
| **Visual Studio 2026** | Bottone "Delegate this task to the GitHub Copilot cloud agent" |

> L'IDE può pushare local changes prima di delegare.

---

### 7. GitHub CLI — `gh agent-task create`

| Trigger | Dettaglio |
|---|---|
| **Comando** | `gh agent-task create "prompt"` |
| **Flag** | `--base`, `--repo`, `--follow` |
| **Minimo** | GitHub CLI v2.80.0+ |

```bash
gh agent-task create "Implement comprehensive unit tests for auth module" \
  --repo OWNER/REPO \
  --base main \
  --follow
```

---

### 8. GitHub MCP Server — `create_pull_request_with_copilot`

| Trigger | Dettaglio |
|---|---|
| **Tool MCP** | `create_pull_request_with_copilot` dal remote GitHub MCP server |
| **Dove** | Qualsiasi IDE/tool con supporto MCP remoto |
| **Effetto** | Draft PR → lavoro asincrono → review request |

---

### 9. Security Campaign — Alert Assignment

| Trigger | Dettaglio |
|---|---|
| **Assign security alert** | Assegnare alert di code scanning a Copilot da una security campaign |
| **Effetto** | Copilot produce fix per le vulnerabilità → PR |

---

### 10. New Repository Prompt

| Trigger | Dettaglio |
|---|---|
| **Prompt field** | Nel form "Create new repository" su github.com |
| **Effetto** | Copilot seed il nuovo repo con codice iniziale → PR |

---

### 11. Integrazioni Esterne (Third Party)

| Integrazione | Trigger |
|---|---|
| **Jira** | Inviare work item a Copilot da Jira workspace |
| **Slack** | Prompt da canale Slack → PR |
| **Microsoft Teams** | Prompt da canale Teams → PR |
| **Linear** | Inviare issue Linear a Copilot → PR |
| **Azure Boards** | Inviare work item da Azure DevOps → PR |
| **Raycast** | "Assign Issues to Copilot" / "Create Task" da launcher Raycast |

> Le integrazioni esterne supportano **solo** la creazione diretta di PR (non research/plan/iterate).

---

### 12. GraphQL API (programmatico)

| Mutation | Uso |
|---|---|
| `createIssue` | Creare issue e assegnarla a Copilot |
| `updateIssue` | Aggiornare issue esistente con assignment a Copilot |
| `addAssigneesToAssignable` | Aggiungere Copilot come assignee (mantenendo altri) |
| `replaceActorsForAssignable` | Sostituire assignee con Copilot |

```graphql
mutation {
  createIssue(input: {
    repositoryId: "REPO_ID",
    title: "Implement feature X",
    body: "Details...",
    assigneeIds: ["BOT_ID"],
    agentAssignment: {
      targetRepositoryId: "REPO_ID",
      baseRef: "main",
      customInstructions: "Follow TDD approach",
      customAgent: "test-specialist",
      model: ""
    }
  }) {
    issue { id title }
  }
}
```

> Header richiesto: `GraphQL-Features: issues_copilot_assignment_api_support,coding_agent_model_selection`

---

### 13. REST API (programmatico)

| Endpoint | Metodo | Uso |
|---|---|---|
| `/repos/{owner}/{repo}/issues` | POST | Creare issue con assignment |
| `/repos/{owner}/{repo}/issues/{number}` | PATCH | Aggiornare issue con assignment |
| `/repos/{owner}/{repo}/issues/{number}/assignees` | POST | Aggiungere Copilot come assignee |

---

## Riepilogo per Scenario Pipeline

| Scenario | Trigger consigliato |
|---|---|
| **CI/CD automatizzato** | REST/GraphQL API → issue assignment |
| **Code review automatica** | `@copilot` mention su PR |
| **Triage automatico** | Webhook + REST API → assign issue a Copilot |
| **Security fix automatizzato** | Security campaign alert assignment |
| **Cross-tool (Azure DevOps, Jira)** | Integrazione nativa o API |
| **CLI scripting** | `gh agent-task create` o `gh issue edit --add-assignee` |
| **IDE-driven delegation** | Delegate to Cloud Agent button |

---

## Customizzazione del Comportamento

| Meccanismo | Descrizione |
|---|---|
| **Custom Instructions** | `.github/copilot-instructions.md` nel repo |
| **Custom Agents** | `.github/agents/AGENT-NAME.agent.md` (repo, org, enterprise) |
| **Skills** | `.github/skills/SKILL-NAME/SKILL.md` |
| **MCP Servers** | Estendere capacità agent con tool esterni |
| **Hooks** | Script shell eseguiti in punti chiave dell'esecuzione agent |
| **Setup Steps** | `.github/copilot-setup-steps.yml` per ambiente dev |
| **Firewall** | Controllare domini accessibili dall'agent |

---

## Riferimenti

- [About Copilot Cloud Agent](https://docs.github.com/en/copilot/concepts/agents/cloud-agent/about-cloud-agent)
- [Create a PR](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/cloud-agent/create-a-pr)
- [Make changes to existing PR](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/cloud-agent/make-changes-to-an-existing-pr)
- [Custom Agents](https://docs.github.com/en/copilot/concepts/agents/cloud-agent/about-custom-agents)
- [Agent Skills](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/cloud-agent/add-skills)
