# Auto-Triage Issues

When a new issue is opened in this repository:

1. Read the issue title and body carefully
2. Classify the issue as one of: `bug`, `feature`, `question`, `docs`, `security`
3. Apply the appropriate type label
4. Assess priority based on impact and urgency:
   - `priority/high` — security issues, data loss, blocking bugs
   - `priority/medium` — functional bugs, important features
   - `priority/low` — cosmetic issues, nice-to-haves, documentation
5. If classified as `security`:
   - Apply the `security` label
   - Add a comment: "This issue has been flagged as security-related and escalated."
6. If classified as `bug` and the body contains a stack trace or error message:
   - Search the codebase for files related to the error
   - Add a comment listing the likely affected files and suggesting where to look
7. If classified as `question` and the answer can be found in docs/ or README.md:
   - Reply with the relevant information
   - Suggest closing the issue
