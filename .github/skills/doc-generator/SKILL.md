---
name: doc-generator
description: >
  Generates branded project documentation from codebase analysis.
  Uses HTML templates with configurable branding (logo, colors, fonts).
  Invoke with /doc-generator when you need to produce professional docs.
allowed-tools:
  - read
  - edit
license: MIT
---

# Doc Generator Skill

Generate professional, branded documentation from project analysis.

## Step 1: Read Configuration

Read `.github/skills/doc-generator/config.yaml` for all settings:
- `branding.*` — company name, logo path, colors, fonts
- `document.*` — title, subtitle, author, version, date
- `sections` — ordered list of sections to generate
- `output.*` — format (html) and file path

## Step 2: Read Template

Read `.github/skills/doc-generator/templates/project-docs.html`.
The template uses `{{placeholder}}` markers:

| Placeholder | Source |
|-------------|--------|
| `{{company}}` | config → branding.company |
| `{{logo_src}}` | config → branding.logo |
| `{{primary_color}}` | config → branding.primary_color |
| `{{secondary_color}}` | config → branding.secondary_color |
| `{{accent_color}}` | config → branding.accent_color |
| `{{font_family}}` | config → branding.font_family |
| `{{heading_font}}` | config → branding.heading_font |
| `{{title}}` | config → document.title |
| `{{subtitle}}` | config → document.subtitle |
| `{{author}}` | config → document.author |
| `{{version}}` | config → document.version |
| `{{date}}` | config → document.date ("auto" = today) |
| `{{toc}}` | Auto-generated from section headings |
| `{{content}}` | All generated sections as HTML |

## Step 3: Generate Content per Section

For each section in `config.yaml → sections`, generate HTML content:

### project-overview
- Read `README.md` and `.csproj` / `pom.xml`
- Extract: project name, description, tech stack, purpose
- Format: introductory paragraph + tech stack table

### architecture
- Read all folders: Controllers/, Services/, Models/, Data/
- Describe layer architecture (API → Service → Data)
- List key design decisions
- Include text-based architecture diagram if possible

### api-reference
- Find all files matching `*Controller.cs` or `*Controller.java`
- For each controller, for each method with `[Http*]` or `@*Mapping`:
  - Extract: HTTP method, route, parameters, return type
  - Extract XML doc / Javadoc comment if present
- Format as HTML table grouped by controller:

```html
<h3>EmployeeController</h3>
<table>
  <tr><th>Method</th><th>Route</th><th>Parameters</th><th>Response</th></tr>
  <tr><td>GET</td><td>/api/employees</td><td>-</td><td>List&lt;Employee&gt;</td></tr>
  ...
</table>
```

### data-model
- Find all files in Models/ (C#) or model/ (Java)
- For each entity: class name, properties with types, annotations
- Identify relationships (navigation properties, FK)
- Format as table per entity + relationships list

### coding-conventions
- Read `.github/copilot-instructions.md`
- Format rules as a structured list with categories
- Add examples where helpful

### getting-started
- Extract from README.md or generate:
  - Prerequisites (runtime version, tools)
  - Clone command
  - Restore/build command
  - Run command
  - Swagger/API URL
- Format as numbered steps with code blocks

## Step 4: Build Table of Contents

Generate an ordered list linking to each section:
```html
<ol>
  <li><a href="#project-overview">Project Overview</a></li>
  <li><a href="#architecture">Architecture</a></li>
  ...
</ol>
```

## Step 5: Assemble and Write

1. Replace ALL `{{placeholders}}` in the template with values
2. If `{{date}}` is "auto", use today's date
3. If `{{logo_src}}` is a file path, keep as-is (browser will resolve)
4. Inject `{{toc}}` and `{{content}}`
5. Verify: no `{{...}}` markers remain in output
6. Write to path specified in `config.yaml → output.path`
7. Report what was generated and any sections with limited content

## Quality Rules
- Every section must have REAL content from the project
- No leftover `{{placeholder}}` markers in the output
- HTML must be valid and semantic (proper heading hierarchy)
- Code examples must be syntax-highlighted in `<pre>` blocks
- Tables must have header rows
- Output must be print-ready (@media print styles are in the template)
