# GitHub Copilot (Enterprise) — CLI Tool Permissions Reference

> Guida ai permessi tool configurabili per **GitHub Copilot** in modalità CLI e pipeline (GitHub Actions).
> Il modello sottostante viene scelto in base al workflow di lavoro.

---

## File Operations

| Permission flag | Descrizione |
|---|---|
| `read_file` | Leggere contenuto di file nel workspace |
| `create_file` | Creare nuovi file |
| `replace_string_in_file` | Modificare file esistenti (singola sostituzione) |
| `multi_replace_string_in_file` | Modifiche multiple in uno o più file |
| `list_dir` | Listare contenuti directory |
| `view_image` | Visualizzare file immagine (png, jpg, gif, webp) |

## Search Operations

| Permission flag | Descrizione |
|---|---|
| `grep_search` | Ricerca testo/regex nei file del workspace |
| `file_search` | Trovare file per glob pattern |
| `semantic_search` | Ricerca semantica nel codebase |

## Terminal / Execution

| Permission flag | Descrizione |
|---|---|
| `run_in_terminal` | Eseguire comandi shell (sync/async) |
| `get_terminal_output` | Leggere output da sessione terminale |
| `send_to_terminal` | Inviare input a terminale interattivo |
| `kill_terminal` | Terminare sessione terminale |

## Testing

| Permission flag | Descrizione |
|---|---|
| `runTests` | Eseguire unit test (run/coverage mode) |

## Web / Fetch

| Permission flag | Descrizione |
|---|---|
| `fetch_webpage` | Fetch e analisi contenuto pagine web |

## Code Intelligence

| Permission flag | Descrizione |
|---|---|
| `vscode_listCodeUsages` | Trovare tutti gli usi/riferimenti di un simbolo |
| `vscode_renameSymbol` | Rinominare simbolo nel workspace (language-aware) |
| `get_errors` | Ottenere errori di compilazione/lint |

## Subagent / Orchestration

| Permission flag | Descrizione |
|---|---|
| `runSubagent` | Lanciare subagent per task complessi |
| `search_subagent` | Subagent specializzato per esplorazione codebase |
| `execution_subagent` | Subagent specializzato per esecuzione comandi |

## MCP Server Tools

| Permission flag | Descrizione |
|---|---|
| `mcp_<server>_<tool>` | Tool specifico di un MCP server registrato |

> Esempio: `mcp_github_github_create_pull_request`, `mcp_pylance_mcp_s_pylanceDocString`

## Notebook

| Permission flag | Descrizione |
|---|---|
| `edit_notebook_file` | Modificare celle notebook |
| `run_notebook_cell` | Eseguire celle notebook |
| `copilot_getNotebookSummary` | Ottenere sommario notebook |

## Misc

| Permission flag | Descrizione |
|---|---|
| `manage_todo_list` | Gestire todo list di sessione |
| `memory` | Leggere/scrivere note persistenti |

---

## Configurazione in Pipeline (GitHub Actions)

### `copilot-setup-steps.yml` — ambiente e dipendenze

```yaml
# .github/copilot-setup-steps.yml
steps:
  - uses: actions/setup-node@v4
    with:
      node-version: "20"
  - run: npm ci
```

### Tool permissions via flag CLI

```bash
# Permettere solo lettura e test
--allow-tool=read_file --allow-tool=grep_search --allow-tool=runTests

# Permettere tutto tranne terminale
--allow-tool=read_file --allow-tool=create_file --allow-tool=replace_string_in_file

# Esecuzione completa (build + deploy)
--allow-tool=read_file --allow-tool=run_in_terminal --allow-tool=create_file
```

### Tool permissions via YAML (settings)

```yaml
# .github/copilot-settings.yml
allowedTools:
  - read_file
  - create_file
  - replace_string_in_file
  - grep_search
  - file_search
  - runTests
  - run_in_terminal
  - get_errors

deniedTools:
  - kill_terminal
  - send_to_terminal
```

---

## Scenari tipici

### Read-only analysis (code review, audit)
```yaml
allowedTools:
  - read_file
  - grep_search
  - file_search
  - semantic_search
  - list_dir
  - get_errors
  - vscode_listCodeUsages
```

### Test & validation
```yaml
allowedTools:
  - read_file
  - grep_search
  - runTests
  - get_errors
  - run_in_terminal
  - get_terminal_output
```

### Full development (story implementation)
```yaml
allowedTools:
  - read_file
  - create_file
  - replace_string_in_file
  - multi_replace_string_in_file
  - grep_search
  - file_search
  - list_dir
  - run_in_terminal
  - get_terminal_output
  - runTests
  - get_errors
  - vscode_listCodeUsages
  - vscode_renameSymbol
```

> **Nota:** L'ordine di precedenza è **deny > allow**. I tool non elencati in `allowedTools` vengono bloccati per default quando la lista è specificata.
