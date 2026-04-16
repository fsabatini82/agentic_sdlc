---
applyTo: "**/*.cs"
---

# C# API Code Standards

- All controller actions must be `async Task<ActionResult<T>>`
- Use `[ProducesResponseType]` attributes on every action
- Inject services via constructor — never instantiate directly
- Max 5 actions per controller — split if needed
- No business logic in controllers — delegate to service layer
- Return `IActionResult` or `ActionResult<T>`, not concrete types
- Use `[FromBody]`, `[FromQuery]`, `[FromRoute]` explicitly
- Add XML doc comments on all public API methods
