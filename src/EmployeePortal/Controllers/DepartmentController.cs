using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeePortal.Data;
using EmployeePortal.Models;

namespace EmployeePortal.Controllers;

/// <summary>
/// Department management — this controller is intentionally CLEAN.
/// It serves as contrast to show that not everything needs fixing.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly PortalDbContext _context;
    private readonly ILogger<DepartmentController> _logger;

    public DepartmentController(PortalDbContext context, ILogger<DepartmentController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetAll()
    {
        _logger.LogInformation("Retrieving all departments");
        var departments = await _context.Departments.ToListAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> GetById(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
            return NotFound();
        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<Department>> Create([FromBody] Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Department {Name} created with ID {Id}", department.Name, department.Id);
        return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Department updated)
    {
        var existing = await _context.Departments.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Name = updated.Name;
        existing.Code = updated.Code;
        existing.ManagerName = updated.ManagerName;
        existing.MaxHeadcount = updated.MaxHeadcount;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
            return NotFound();

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Department {Id} deleted", id);
        return NoContent();
    }
}
