# EX11: Model Selection — Guida Pratica

## Type: Reference / Discussione
## Difficulty: Concettuale

---

## Quando Scegliere Quale Modello

| Task Type | Recommended | Alternative | Cost |
|-----------|------------|-------------|------|
| Bug fix (single file) | **Sonnet 4.6** | GPT-5.4 | $ - $$ |
| Multi-file feature | **Opus 4.6** | GPT-5.4 | $$ - $$$ |
| Refactoring | **GPT-5.4** | Sonnet 4.6 | $$ |
| Test generation | **Sonnet 4.6** | Haiku 4.5 | $ |
| Security fix | **Auto** | — | $$ |
| Documentation | **GPT-5.4** | Sonnet 4.6 | $$ |
| Complex architecture | **Opus 4.6** | — | $$$ |
| Quick prototype | **Sonnet 4.6** | Haiku 4.5 | $ |
| Bulk autonomous (issue→PR) | **Codex (5.3)** | Haiku 4.5 | $ |
| CI/CD pipeline scripting | **Codex (5.3)** | Haiku 4.5 | $ |
| Trivial (typo, rename) | **Haiku 4.5** | — | $ |

## Fasce di Costo

| Fascia | Modelli |
|--------|---------|
| **$** | Haiku 4.5, Codex (GPT-5.3), GPT-4.1 |
| **$$** | Sonnet 4.6, GPT-5.4, Gemini 2.5 Pro |
| **$$$** | Opus 4.6 |

## Rule of Thumb

> **Opus for thinking hard. Sonnet for thinking fast. GPT for structured output.**

Start with "Auto" — Copilot routes to the best model. Override only when:
(a) you need consistent output format, (b) cost matters, (c) task is clearly single-domain.

## Context Window

| Modello | Context |
|---------|---------|
| Opus 4.6 / Sonnet 4.6 | 200K token |
| GPT-5.4 | 256K token |
| Codex (5.3) | 512K token |
| **Gemini 2.5 Pro** | **1M+ token** |

> Gemini 2.5 Pro: da considerare per codebase molto ampie dove serve analizzare decine di file in un singolo passaggio.

> GPT-4.1: modello legacy, utile solo per task molto semplici. Per qualsiasi task strutturato, Sonnet 4.6 e GPT-5.4 sono superiori.
