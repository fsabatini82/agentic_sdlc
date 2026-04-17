# EX09: PR Changelog + Release Notes

## Type: CLI + GitHub Actions
## Difficulty: Medio
## Duration: 10 min

---

## Scenario
Al push di un tag `v*`, generare automaticamente changelog e release notes dalle PR merged.

## Workflow

```yaml
name: Release Notes
on:
  push:
    tags: ['v*']
jobs:
  changelog:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v5
        with:
          fetch-depth: 0
      - uses: actions/setup-node@v4
      - run: npm install -g @github/copilot
      - env:
          COPILOT_GITHUB_TOKEN: ${{ secrets.COPILOT_PAT }}
        run: |
          copilot -p "Review the git log since the previous tag.
          Group changes by: Features, Bug Fixes, Breaking Changes, Docs.
          For each: PR title, author, linked issues.
          Generate CHANGELOG.md entry (Keep-a-Changelog format).
          Also write release-notes.md for GitHub Release body." \
            --allow-tool='shell(git:*)' \
            --allow-tool=write \
            --no-ask-user
```

## Steps (demo)
1. Creare qualche commit/PR nel repo
2. Taggare: `git tag v1.0.0 && git push --tags`
3. Osservare il workflow che genera changelog

## Risultato Atteso
```markdown
## [1.0.0] - 2026-04-16

### Features
- Add pagination to GET /api/employees (#1) @fabio.sabatini

### Bug Fixes
- Fix GET side effect in EmployeeController (#2) @copilot

### Security
- Fix SQL injection in ReportController (#3) @copilot
```
