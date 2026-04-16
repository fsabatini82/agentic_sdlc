---
description: "Generate branded project documentation from codebase analysis"
---

# Generate Project Documentation

Use the **@doc-writer** agent to analyze this project and generate
branded documentation using the **/doc-generator** skill.

## Steps

### 1. Analyze the project
Read all source files: Controllers, Models, Data, Program.cs, README.md,
.csproj, and .github/copilot-instructions.md. Build a complete picture
of the tech stack, architecture, API endpoints, and data model.

### 2. Read configuration
Read `.github/skills/doc-generator/config.yaml` for branding and sections.
Read `.github/skills/doc-generator/templates/project-docs.html` for the template.

### 3. Generate content
For EACH section listed in config.yaml, generate real content from the
project analysis. Use the /doc-generator skill instructions for format
details per section.

### 4. Assemble and write
Replace all {{placeholders}} in the template. Write the final HTML to
the output path specified in config.yaml.

## Quality Check
- Every section has real content from this project
- No {{placeholder}} markers remain
- API reference lists ALL endpoints with correct routes
- Data model covers ALL entity classes
