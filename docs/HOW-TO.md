# HOW-TO — GitHub Copilot: CLI & Agentic Workflow

> Guida operativa step-by-step per installare la CLI, eseguire batch agentici locali, comprendere l'architettura server-side e creare il primo Agentic Workflow (Cloud Agent).
>
> **Riferimento ufficiale:** <https://docs.github.com/en/copilot>

---

## Indice

- [Parte 1 — Copilot CLI: Installazione e Configurazione](#parte-1--copilot-cli-installazione-e-configurazione)
  - [1.1 Prerequisiti](#11-prerequisiti)
  - [1.2 Metodi di installazione](#12-metodi-di-installazione)
  - [1.3 Autenticazione](#13-autenticazione)
  - [1.4 Uso interattivo (locale)](#14-uso-interattivo-locale)
  - [1.5 Uso programmatico — Batch agentici locali](#15-uso-programmatico--batch-agentici-locali)
  - [1.6 Gestione permessi tool](#16-gestione-permessi-tool)
  - [1.7 Custom Model Provider (opzionale)](#17-custom-model-provider-opzionale)
  - [1.8 MCP Server nella CLI](#18-mcp-server-nella-cli)
  - [1.9 Architettura server-side — Come funziona il "giro" Cloud](#19-architettura-server-side--come-funziona-il-giro-cloud)
- [Parte 2 — Agentic Workflow (Cloud Agent)](#parte-2--agentic-workflow-cloud-agent)
  - [2.1 Cos'è il Cloud Agent](#21-cosè-il-cloud-agent)
  - [2.2 Piani e requisiti](#22-piani-e-requisiti)
  - [2.3 Attivazione — Step-by-step](#23-attivazione--step-by-step)
  - [2.4 Configurazione del repository](#24-configurazione-del-repository)
  - [2.5 Creare il primo Agentic Workflow](#25-creare-il-primo-agentic-workflow)
  - [2.6 GitHub Agentic Workflows](#26-github-agentic-workflows)
  - [2.7 Modalità avanzate: Research, Plan, Iterate](#27-modalità-avanzate-research-plan-iterate)
  - [2.8 Monitoraggio e sessioni](#28-monitoraggio-e-sessioni)

---

# Parte 1 — Copilot CLI: Installazione e Configurazione

## 1.1 Prerequisiti

| Requisito | Dettaglio |
|---|---|
| **Subscription attiva** | Copilot Pro, Pro+, Business o Enterprise |
| **Node.js** | Versione **22+** (richiesto per installazione via npm) |
| **PowerShell** | Versione **6+** su Windows |
| **Sistema operativo** | Linux, macOS, Windows (PowerShell nativo o WSL) |

## 1.2 Metodi di installazione

### Metodo 1 — npm (consigliato, cross-platform)

```bash
npm install -g @github/copilot
```

### Metodo 2 — WinGet (Windows)

```powershell
winget install GitHub.Copilot
```

### Metodo 3 — Homebrew (macOS / Linux)

```bash
brew install copilot-cli
```

### Metodo 4 — Script di installazione (Linux / macOS)

```bash
curl -fsSL https://gh.io/copilot-install | bash
```

### Metodo 5 — Download diretto

Scaricare il binario dalla pagina [github/copilot-cli/releases](https://github.com/github/copilot-cli/releases) ed estrarlo nel `PATH`.

### Verifica installazione

```bash
copilot --version
```

## 1.3 Autenticazione

### Metodo interattivo (consigliato)

Avviare la CLI e usare il comando slash integrato:

```bash
copilot
# all'interno della sessione:
/login
```

Si aprirà un flusso OAuth nel browser.

### Metodo via Personal Access Token (PAT)

Creare un PAT con il permesso **"Copilot Requests"** e impostare una delle seguenti variabili d'ambiente (in ordine di priorità):

```bash
# Sceglierne UNA:
export COPILOT_GITHUB_TOKEN="ghp_..."
export GH_TOKEN="ghp_..."
export GITHUB_TOKEN="ghp_..."
```

Su Windows (PowerShell):

```powershell
$env:COPILOT_GITHUB_TOKEN = "ghp_..."
```

> **Nota:** Il PAT con scope `copilot` è sufficiente per la CLI. Per operazioni su repository serve anche lo scope `repo`.

## 1.4 Uso interattivo (locale)

```bash
copilot
```

Si apre una sessione REPL in cui digitare prompt in linguaggio naturale. Comandi utili:

| Comando | Descrizione |
|---|---|
| `Shift+Tab` | Attiva **plan mode** — Copilot propone un piano prima di eseguire |
| `@file` o `@folder` | Referenzia file/cartelle come contesto |
| `!` + comando | Esegue un comando shell direttamente |
| `/add-dir <path>` | Aggiunge una directory al contesto di lavoro |
| `/compact` | Compatta la conversazione per risparmiare contesto |
| `/context` | Mostra il contesto attuale |
| `/usage` | Mostra statistiche di utilizzo token |
| `/mcp add <server>` | Aggiunge un server MCP |
| `/agent <name>` | Usa un custom agent specifico |
| `Ctrl+T` | Toggle del reasoning esteso |
| `/login` | Autenticazione interattiva |
| `--resume` / `--continue` | Riprende sessione precedente |

## 1.5 Uso programmatico — Batch agentici locali

La modalità **programmatica** permette di eseguire task automatizzati (batch) senza interazione:

```bash
copilot -p "Descrizione del task da eseguire" \
  --allow-tool="shell(git)" \
  --allow-tool="write" \
  --allow-tool="read"
```

### Esempio: Batch per aggiungere test unitari

```bash
copilot -p "Aggiungi test unitari per tutti i metodi pubblici in src/Services/UserService.cs. Usa xUnit e FluentAssertions." \
  --allow-tool="write" \
  --allow-tool="shell(dotnet)" \
  --allow-tool="read"
```

### Esempio: Batch per fix lint

```bash
copilot -p "Esegui il linter e correggi automaticamente tutti i warning in src/" \
  --allow-all-tools
```

### Esempio: Batch con custom agent

```bash
copilot -p "Refactoring del modulo auth" \
  --agent="security-reviewer" \
  --allow-tool="write" \
  --allow-tool="read"
```

### Flag utili per batch

| Flag | Descrizione |
|---|---|
| `-p "prompt"` | Prompt in modalità non-interattiva (esce al completamento) |
| `--no-ask-user` | **Sopprime TUTTI i prompt interattivi** (essenziale per CI/CD) |
| `--allow-all-tools` / `--yolo` | Approva automaticamente tutti i tool |
| `--allow-tool="<tool>"` | Approva un tool specifico |
| `--deny-tool="<tool>"` | Blocca un tool specifico |
| `--agent="<name>"` | Usa un custom agent |
| `--resume` | Riprende l'ultima sessione |
| `--continue` | Continua la sessione corrente |

### Pattern tool permissions

```bash
# --- Shell commands ---
--allow-tool='shell'               # TUTTI i comandi shell (pericoloso!)
--allow-tool='shell(git:*)'        # tutti i sottocomandi git (git push, pull, ma NON gitea)
--allow-tool='shell(dotnet:*)'     # tutti i sottocomandi dotnet
--allow-tool='shell(git commit)'   # SOLO git commit
--deny-tool='shell(rm:*)'          # blocca tutti i comandi rm

# --- File operations ---
--allow-tool='read'                # lettura di qualsiasi file
--allow-tool='write'               # scrittura/modifica di qualsiasi file
--allow-tool='write(.github/copilot-instructions.md)'  # scrittura limitata a un file

# --- Web ---
--allow-tool='web_fetch'           # fetch di pagine web
--allow-tool='web_search'          # ricerca web

# --- MCP Servers ---
--allow-tool='MCP_SERVER'          # TUTTI i tool di un server MCP
--allow-tool='MCP_SERVER(tool)'    # tool specifico di un server MCP
--allow-tool='github(get_issue)'   # esempio: solo get_issue dal server github

# --- Global ---
--allow-all-tools                  # alias: --yolo — approva tutto (usare solo in CI/sandbox)
--allow-all-paths                  # permette accesso a qualsiasi path del filesystem
```

> **Regola di precedenza:** `--deny-tool` ha **SEMPRE priorità** su `--allow-tool`, anche quando si usa `--allow-all-tools`. Un deny esplicito non può essere sovrascritto.

> **Sintassi `:*`:** Il suffisso `:*` matcha il comando seguito da spazio — `shell(git:*)` matcha `git push` e `git pull` ma **NON** `gitea`.

## 1.6 Gestione permessi tool

La CLI offre tre livelli di controllo:

1. **Default** — Chiede conferma per ogni operazione potenzialmente distruttiva
2. **Allow specifico** — `--allow-tool="write"` pre-approva tool singoli
3. **Allow all** — `--allow-all-tools` (alias `--yolo`) approva tutto senza conferma

> **⚠️ Sicurezza:** Usare `--allow-all-tools` solo in ambienti controllati (CI/CD, sandbox). Mai in produzione.

## 1.7 Custom Model Provider (opzionale)

Per usare un modello diverso dal default (Claude Sonnet 4.6), configurare le variabili d'ambiente:

```bash
export COPILOT_PROVIDER_BASE_URL="https://your-provider.example.com/v1"
export COPILOT_PROVIDER_TYPE="openai"      # openai | azure-openai | anthropic | ollama
export COPILOT_PROVIDER_API_KEY="sk-..."
export COPILOT_MODEL="gpt-4o"              # nome del modello
```

Provider supportati: **OpenAI-compatible**, **Azure OpenAI**, **Anthropic**, **Ollama** (locale).

## 1.8 MCP Server nella CLI

Aggiungere server MCP (Model Context Protocol) per estendere le capacità della CLI:

```bash
# Nella sessione interattiva:
/mcp add <nome-server>
```

La configurazione viene salvata in `~/.copilot/mcp-config.json`.

## 1.9 Architettura server-side — Come funziona il "giro" Cloud

> **Domanda chiave:** Serve un container o una VM dedicata?
>
> **Risposta: NO.** Il Cloud Agent gira in un **ambiente GitHub Actions effimero** gestito interamente da GitHub. Non è necessario provisioning di container, VM o infrastruttura propria.

### Come funziona

```
┌─────────────────────────────────────────────────────────┐
│                    TRIGGER                               │
│  (Issue assign / Agents panel / @copilot / CLI / IDE)   │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│              GITHUB CLOUD INFRASTRUCTURE                 │
│                                                          │
│  1. GitHub alloca un runner GitHub Actions effimero       │
│     (Ubuntu latest, oppure Windows se configurato)       │
│                                                          │
│  2. Esegue copilot-setup-steps.yml (se presente)         │
│     → installa dipendenze, tool, SDK                     │
│                                                          │
│  3. Clona il repository nel runner                        │
│                                                          │
│  4. Il Cloud Agent (LLM) lavora autonomamente:           │
│     → legge codice, pianifica, scrive, testa, committa   │
│                                                          │
│  5. Crea una Pull Request in draft                       │
│                                                          │
│  6. Richiede review all'autore                           │
│                                                          │
│  7. Il runner viene distrutto (effimero)                 │
└─────────────────────────────────────────────────────────┘
```

### Cosa NON serve

- ❌ Nessuna VM o container da gestire
- ❌ Nessun processo in-process nel tuo GitHub Enterprise Server
- ❌ Nessuna infrastruttura da mantenere

### Cosa SI può personalizzare

- ✅ **Runner type** — Standard, larger runner, o self-hosted (via `copilot-setup-steps.yml` → `runs-on`)
- ✅ **SO** — Ubuntu (default) o Windows 64-bit
- ✅ **Dipendenze** — Pre-installabili via `copilot-setup-steps.yml`
- ✅ **Variabili d'ambiente** — Via GitHub Actions environment `copilot`
- ✅ **Firewall** — Personalizzabile o disabilitabile
- ✅ **Proxy** — Configurabile su self-hosted runner (`https_proxy`, `http_proxy`, `no_proxy`)

### Costi

L'utilizzo del Cloud Agent consuma:
- **GitHub Actions minutes** per il runner
- **Premium requests** del piano Copilot

---

# Parte 2 — Agentic Workflow (Cloud Agent)

## 2.1 Cos'è il Cloud Agent

Il **Copilot Cloud Agent** (precedentemente "Coding Agent") è un agente software AI che lavora **autonomamente** su task di sviluppo:

- Analizza issue e requisiti
- Esplora il codebase per capire il contesto
- Pianifica le modifiche
- Scrive codice, esegue test e linter
- Crea una Pull Request con le modifiche
- Itera sui feedback della review

Il tutto avviene in un ambiente **effimero GitHub Actions** — nessuna infrastruttura da gestire.

## 2.2 Piani e requisiti

| Piano | Cloud Agent | Note |
|---|---|---|
| **Copilot Pro** | ✅ Abilitato di default | — |
| **Copilot Pro+** | ✅ Abilitato di default | Supporta anche agent di terze parti |
| **Copilot Business** | ⚠️ Disabilitato di default | L'admin dell'organizzazione deve abilitarlo |
| **Copilot Enterprise** | ⚠️ Disabilitato di default | L'admin dell'enterprise/org deve abilitarlo |

## 2.3 Attivazione — Step-by-step

### Per utenti Pro / Pro+

Nessuna azione richiesta. Il Cloud Agent è già attivo.

Per verificare o disabilitare per singoli repository:
1. Vai su **github.com** → **Settings** (del tuo profilo)
2. Nella sezione **Copilot**, cerca l'opzione **Cloud Agent**
3. Abilita/disabilita per repository specifici

### Per organizzazioni (Business / Enterprise)

#### Step 1 — Abilitazione a livello enterprise (se applicabile)

1. Accedi come **Enterprise Admin** su `github.com/enterprises/<enterprise>/settings`
2. Vai su **Policies** → **Copilot**
3. Cerca la policy **Copilot Cloud Agent**
4. Imposta su **Enabled** (o "No policy" per delegare alle org)

#### Step 2 — Abilitazione a livello organizzazione

1. Accedi come **Org Owner** su `github.com/orgs/<org>/settings`
2. Vai su **Copilot** → **Policies**
3. Cerca **Cloud Agent** e impostalo su **Enabled**
4. Scegli se abilitare per **tutti i repository** o per un sottoinsieme

#### Step 3 — Verifica per singolo repository

1. Accedi al repository → **Settings** → **General**
2. Nella sezione **Features**, verifica che **Copilot Cloud Agent** sia attivo

> **Nota:** Un admin dell'organizzazione può fare opt-out di repository specifici anche se la policy generale è "Enabled".

## 2.4 Configurazione del repository

### File di configurazione chiave

Tutti i file vanno nella directory `.github/` del repository:

```
.github/
├── copilot-instructions.md          # Istruzioni custom per Copilot
├── agents/
│   └── my-agent.agent.md            # Custom agent definitions
├── hooks/
│   └── pre-tool-use.json            # Hook per eventi del ciclo di vita
├── skills/
│   └── my-skill/
│       └── SKILL.md                 # Skill personalizzate
└── workflows/
    └── copilot-setup-steps.yml      # Setup dell'ambiente effimero
```

### A — Custom Instructions (`copilot-instructions.md`)

Istruzioni condivise che Copilot legge prima di ogni task nel repository:

```markdown
# Repository Custom Instructions

## Convenzioni di codice
- Usa C# 12 con nullable reference types abilitati
- Segui il pattern CQRS per i command/query handler
- Ogni servizio deve avere test unitari con copertura > 80%

## Struttura progetto
- I servizi backend sono in src/<NomeServizio>/
- I test sono in src/Tests/<NomeServizio>.Tests/

## Dipendenze
- ORM: Entity Framework Core 9
- Testing: xUnit + FluentAssertions + NSubstitute
- Logging: Serilog
```

### B — Copilot Setup Steps (`copilot-setup-steps.yml`)

Configura l'ambiente del runner prima che Copilot inizi a lavorare:

```yaml
name: "Copilot Setup Steps"

on:
  workflow_dispatch:
  push:
    paths:
      - .github/workflows/copilot-setup-steps.yml
  pull_request:
    paths:
      - .github/workflows/copilot-setup-steps.yml

jobs:
  # Il job DEVE chiamarsi "copilot-setup-steps"
  copilot-setup-steps:
    runs-on: ubuntu-latest

    permissions:
      contents: read

    steps:
      - name: Checkout code
        uses: actions/checkout@v5

      - name: Setup .NET 10
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: "10.0.x"

      - name: Restore dependencies
        run: dotnet restore src/InGate.sln

      - name: Build solution
        run: dotnet build src/InGate.sln --no-restore
```

**Opzioni personalizzabili nel job:**

| Opzione | Descrizione |
|---|---|
| `steps` | Comandi di setup pre-lavoro |
| `permissions` | Permessi GitHub Actions necessari |
| `runs-on` | Tipo di runner (`ubuntu-latest`, `ubuntu-4-core`, `windows-latest`, label ARC) |
| `services` | Container di servizio (es. database per i test) |
| `timeout-minutes` | Timeout massimo (max `59`) |

> **Importante:** Il file `copilot-setup-steps.yml` deve essere presente nel **branch di default** per essere utilizzato.

### C — Custom Agents (`.github/agents/*.agent.md`)

Definisci agent specializzati per task specifici:

```markdown
---
name: "security-reviewer"
description: "Agente specializzato in review di sicurezza"
---

# Security Reviewer Agent

## Istruzioni
Sei un esperto di sicurezza applicativa. Quando ricevi un task:

1. Analizza il codice per vulnerabilità OWASP Top 10
2. Verifica la gestione corretta di autenticazione e autorizzazione
3. Controlla injection (SQL, XSS, Command)
4. Valuta la gestione dei segreti e delle credenziali
5. Proponi fix con spiegazione del rischio mitigato
```

### D — Hooks (`.github/hooks/*.json`)

Estendi il comportamento del Cloud Agent con hook sul ciclo di vita:

```json
{
  "version": 1,
  "hooks": [
    {
      "event": "preToolUse",
      "tools": ["shell"],
      "script": {
        "bash": "echo 'Verifico il comando prima dell'esecuzione...'",
        "powershell": "Write-Host 'Verifico il comando prima dell''esecuzione...'"
      },
      "cwd": ".",
      "timeoutSec": 30
    }
  ]
}
```

**Eventi hook disponibili:**

| Evento | Quando si attiva |
|---|---|
| `sessionStart` | All'avvio della sessione |
| `sessionEnd` | Alla fine della sessione |
| `userPromptSubmitted` | Quando l'utente invia un prompt |
| `preToolUse` | Prima dell'esecuzione di un tool |
| `postToolUse` | Dopo l'esecuzione di un tool |
| `agentStop` | Quando l'agente si ferma |
| `subagentStop` | Quando un sub-agente si ferma |
| `errorOccurred` | Quando si verifica un errore |

### E — Variabili d'ambiente

Per passare variabili d'ambiente (es. API key per servizi esterni) al Cloud Agent:

1. Vai nel repository → **Settings** → **Environments**
2. Clicca sull'environment **`copilot`**
3. Aggiungi **Environment variables** (valori pubblici) o **Environment secrets** (valori sensibili)

## 2.5 Creare il primo Agentic Workflow

### Metodo 1 — Assegnare una Issue a Copilot (il più comune)

1. Vai nel repository su **github.com**
2. Apri una **Issue** esistente (o creane una nuova con una descrizione chiara del task)
3. Nel pannello destro, clicca **Assignees**
4. Seleziona **Copilot** dalla lista
5. Nella finestra di dialogo:
   - (Opzionale) Aggiungi **istruzioni aggiuntive** nel campo prompt
   - (Opzionale) Scegli il **branch base** da cui partire
   - (Opzionale) Seleziona un **custom agent**
   - (Opzionale) Scegli il **modello AI** (Pro/Pro+)
6. Conferma l'assegnazione
7. Copilot avvia una sessione autonoma → crea branch → lavora → apre una **Draft PR** → ti richiede la **review**

### Metodo 2 — Agents Panel su GitHub.com

1. Clicca l'icona **Agents** (🤖) nella navbar in alto a destra di GitHub
2. Seleziona il **repository** dal dropdown
3. Scrivi un **prompt** descrivendo il task (es. `Create a pull request to add input validation to the registration form`)
4. (Opzionale) Scegli branch base, custom agent, modello
5. Clicca **Start task** (o premi `Enter`)

### Metodo 3 — Da Copilot Chat in VS Code

1. Installa l'estensione **GitHub Pull Requests**
2. Apri **GitHub Copilot Chat** in VS Code
3. Scrivi il prompt del task
4. Clicca il pulsante **"Delegate this task to the GitHub Copilot cloud agent"** (accanto al pulsante Send)
5. Se hai modifiche locali, scegli se includerle o ignorarle
6. Copilot si avvia nel cloud e risponde con il link alla PR

### Metodo 4 — Da GitHub CLI

```bash
gh agent-task create "Implementa validazione input nel form di registrazione"
```

Flag utili:
```bash
gh agent-task create "prompt" \
  --base main \
  --repo owner/repo \
  --follow              # segui i log della sessione in tempo reale
```

> **Nota:** Richiede GitHub CLI v2.80.0+ con il command set `agent-task` (public preview).

### Metodo 5 — Da Copilot Chat su GitHub.com

1. Apri Copilot Chat su github.com
2. Digita `/task` seguito dalla descrizione:
   ```
   /task Create a pull request to implement error handling in the API controllers
   ```
3. Seleziona repository, branch, agent, modello
4. Clicca **Start task**

### Metodo 6 — Via GitHub MCP Server

1. Installa il GitHub MCP server nel tuo IDE
2. Abilita il tool `create_pull_request_with_copilot`
3. Chiedi al tuo agente locale di creare una PR tramite Copilot Cloud

### Metodo 7 — Da Raycast (macOS / Windows)

1. Installa [Raycast](https://www.raycast.com/)
2. Installa l'estensione **GitHub Copilot** per Raycast
3. Cerca "Copilot" → **Create Task** → `Enter`
4. Autentica con GitHub
5. Seleziona repository, scrivi prompt, scegli opzioni
6. `Ctrl+Enter` (Windows) / `Cmd+Enter` (macOS) per avviare

## 2.6 GitHub Agentic Workflows

### Cos'è

Agentic Workflows permette di scrivere automazioni CI/CD in **Markdown** (non YAML). Il CLI `gh aw` converte il Markdown in workflow GitHub Actions standard.

### Installazione

```bash
# Prerequisito: GitHub CLI (gh) installato
gh extension install github/gh-aw
```

### Passo-passo: creare il primo Agentic Workflow

**Step 1 — Scrivi il workflow in Markdown:**

Crea il file `.github/workflows/auto-triage.md`:

```markdown
# Auto-Triage Issues

When a new issue is opened in this repository:
1. Read the issue title and body
2. Classify as: bug | feature | question | security
3. Apply the appropriate label
4. If security: escalate with comment
5. If bug with stack trace: search codebase, comment affected files
```

**Step 2 — Sincronizza (converte MD → Actions YAML):**

```bash
gh aw sync
```

Questo genera automaticamente il workflow YAML corrispondente.

**Step 3 — Testa:**

Crea una issue nel repo e osserva le label applicate e i commenti generati automaticamente.

### Trigger supportati

| Trigger | Descrizione | Shorthand |
|---------|-------------|-----------|
| `issues` | Issue aperta, editata, labellata, chiusa | `on: issue opened` |
| `pull_request` | PR aperta, sincronizzata, merged | `on: pull_request` |
| `issue_comment` | Commento su issue o PR | `on: issue comment` |
| `pull_request_review_comment` | Commento di review su PR | — |
| `discussion_comment` | Commento su GitHub Discussion | — |
| `schedule` | Cron o linguaggio naturale | `on: daily`, `cron: '0 6 * * 1'` |
| `workflow_dispatch` | Lancio manuale da UI o API | `on: workflow_dispatch` |
| `push` | Commit su branch o tag, con glob filter | `on: push to main` |
| `release` | Release pubblicata, creata, pre-released | `on: release published` |
| `slash_command` | `/comando` scritto in un commento | `on: /triage` |
| `label_command` | Label applicata (auto-rimossa per re-trigger) | `on: label "needs-triage"` |
| `workflow_run` | Dopo il completamento di un altro workflow | — |
| `watch` | Repo starred | — |
| `fork` | Repo forkato | — |
| `repository_dispatch` | Evento custom via API | — |

### Trigger modifier

| Modifier | Descrizione |
|----------|-------------|
| `status-comment:` | Posta commento "started/completed" con link al workflow run |
| `skip-if-match:` | Salta il workflow se una query GitHub trova match |
| `skip-if-no-match:` | Salta il workflow se una query GitHub NON trova match |
| `stop-after:` | Disabilita automaticamente il trigger dopo una data (assoluta o relativa) |
| `manual-approval:` | Richiede approvazione via environment protection rule |
| `lock-for-agent:` | Blocca la issue/PR durante l'esecuzione dell'agent |

### Sicurezza

- **Read-only di default** — il workflow non può modificare nulla senza safe outputs espliciti
- **PRs mai auto-merged** — serve sempre review umana
- **Safe outputs** — write operations pre-approvate dichiarate nel workflow

---

## 2.7 Modalità avanzate: Research, Plan, Iterate

Il Cloud Agent supporta anche un flusso **iterativo** (senza creare subito una PR):

### Deep Research

```
Prompt: "Analizza le performance dell'app e suggerisci miglioramenti"
```

Copilot esplora il repository in profondità e restituisce un'analisi ragionata.

### Plan

```
Prompt: "Crea un piano per implementare i miglioramenti di performance più impattanti"
```

Copilot propone un piano dettagliato. Puoi iterare finché non sei soddisfatto.

### Iterate

```
Prompt: "Implementa il piano che abbiamo concordato"
```

Copilot lavora su un branch, puoi:
- Cliccare **Diff** per vedere le modifiche
- Chiedere aggiustamenti con ulteriori prompt
- Quando sei soddisfatto, cliccare **Create pull request**

> **Nota:** Le modalità Research, Plan, Iterate sono disponibili solo su **github.com** (agents panel/tab). Le integrazioni esterne (Slack, Teams, Jira, Azure Boards) supportano solo la creazione diretta di PR.

## 2.8 Monitoraggio e sessioni

### Dove vedere le sessioni attive e passate

- **Agents tab** nel repository (icona 🤖)
- **Agents page**: [github.com/copilot/agents](https://github.com/copilot/agents)
- **Dashboard** di GitHub (sessioni recenti)
- **VS Code** (pannello Copilot)

### Ciclo di vita di una sessione

```
Task ricevuto
    │
    ▼
Setup Steps (copilot-setup-steps.yml)
    │
    ▼
Esplorazione codebase
    │
    ▼
Pianificazione modifiche
    │
    ▼
Scrittura codice + test + lint
    │
    ▼
Commit e push su branch
    │
    ▼
Apertura Draft PR
    │
    ▼
Richiesta review → 📬 Notifica
    │
    ▼
(Iterazioni su feedback nella PR)
    │
    ▼
Merge manuale da parte dello sviluppatore
```

---

## Quick Reference Card

| Cosa vuoi fare | Comando / Azione |
|---|---|
| Installare la CLI | `npm install -g @github/copilot` |
| Autenticarsi | `copilot` → `/login` |
| Batch locale | `copilot -p "task" --allow-all-tools` |
| Abilitare Cloud Agent (Business) | Org Settings → Copilot → Policies → Cloud Agent → Enable |
| Setup ambiente runner | Creare `.github/workflows/copilot-setup-steps.yml` |
| Istruzioni personalizzate | Creare `.github/copilot-instructions.md` |
| Custom agent | Creare `.github/agents/nome.agent.md` |
| Primo Agentic Workflow | Assegnare una Issue a Copilot |
| Task da CLI | `gh agent-task create "prompt"` |
| Task da VS Code | Copilot Chat → Delegate to Cloud Agent |
| Seguire sessioni | github.com/copilot/agents |

---

> **Fonti:**
> - [Install Copilot CLI](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/use-copilot-cli/install-copilot-cli)
> - [Use Copilot CLI](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/use-copilot-cli)
> - [About Cloud Agent](https://docs.github.com/en/copilot/concepts/agents/cloud-agent/about-cloud-agent)
> - [Customize Agent Environment](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/cloud-agent/customize-the-agent-environment)
> - [Access Management](https://docs.github.com/en/copilot/concepts/agents/cloud-agent/access-management)
> - [Create a PR](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/cloud-agent/create-a-pr)
> - [Research, Plan, Iterate](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/cloud-agent/research-plan-iterate)
> - [About Hooks](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/cloud-agent/about-hooks)
