using EmployeePortal.Data;
using EmployeePortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePortal.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly PortalDbContext _context;

    public EmployeeController(PortalDbContext context)
    {
        _context = context;
    }

    // GET api/employees
    [HttpGet]
    public IActionResult GetAll()
    {
        Console.WriteLine("Getting all employees...");

        // No pagination - returns entire table
        var employees = _context.Employees.ToList();

        Console.WriteLine($"Found {employees.Count} employees");
        return Ok(employees);
    }

    // GET api/employees/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        Console.WriteLine($"Getting employee {id}...");

        var employee = _context.Employees.Find(id);

        if (employee == null)
        {
            Console.WriteLine($"Employee {id} not found");
            return NotFound();
        }

        // Side effect in a GET request! Appending to audit trail on read
        employee.AuditTrail = (employee.AuditTrail ?? "") + $"\nViewed at {DateTime.Now}";
        _context.SaveChanges();

        Console.WriteLine($"Returning employee: {employee.FirstName} {employee.LastName}");
        return Ok(employee);
    }

    // POST api/employees
    [HttpPost]
    public IActionResult Create([FromBody] Employee employee)
    {
        Console.WriteLine($"Creating employee: {employee.FirstName} {employee.LastName}");

        // No validation at all - salary can be negative, email can be anything
        _context.Employees.Add(employee);
        _context.SaveChanges();

        // Simulate sending welcome email
        Console.WriteLine($"Sending welcome email to {employee.Email}...");
        Thread.Sleep(500);
        Console.WriteLine("Welcome email sent.");

        Console.WriteLine($"Employee created with ID {employee.Id}");
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    // PUT api/employees/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Employee employee)
    {
        Console.WriteLine($"Updating employee {id}...");

        var existing = _context.Employees.Find(id);
        if (existing == null)
        {
            Console.WriteLine($"Employee {id} not found for update");
            return NotFound();
        }

        // God-method: validate + save + notify + audit all inline
        // Validate
        if (string.IsNullOrEmpty(employee.FirstName))
        {
            Console.WriteLine("Validation failed: FirstName is required");
            return BadRequest("FirstName is required");
        }
        if (string.IsNullOrEmpty(employee.LastName))
        {
            Console.WriteLine("Validation failed: LastName is required");
            return BadRequest("LastName is required");
        }

        // Update fields
        existing.FirstName = employee.FirstName;
        existing.LastName = employee.LastName;
        existing.Email = employee.Email;
        existing.Salary = employee.Salary;
        existing.Department = employee.Department;
        existing.Position = employee.Position;
        existing.IsActive = employee.IsActive;
        existing.HireDate = employee.HireDate;

        // Audit trail inline
        existing.AuditTrail = (existing.AuditTrail ?? "") + $"\nUpdated at {DateTime.Now} by system";
        existing.LastModifiedBy = "system";

        // Save
        try
        {
            _context.SaveChanges();
            Console.WriteLine($"Employee {id} updated successfully");
        }
        catch (Exception)
        {
            // TODO: handle this properly
        }

        // Notify
        Console.WriteLine($"Notifying HR about employee {id} update...");
        Console.WriteLine($"Notifying manager of {existing.Department} about update...");

        return Ok(existing);
    }

    // GET api/employees/department/{department}
    [HttpGet("department/{department}")]
    public IActionResult GetByDepartment(string department)
    {
        Console.WriteLine($"Searching employees in department: {department}");

        var employees = _context.Employees
            .Where(e => e.Department == department)
            .ToList();

        Console.WriteLine($"Found {employees.Count} employees in {department}");
        return Ok(employees);
    }

    // DELETE api/employees/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        Console.WriteLine($"Deleting employee {id}...");

        var employee = _context.Employees.Find(id);
        if (employee == null)
        {
            return NotFound();
        }

        // Hard delete - no soft delete pattern
        _context.Employees.Remove(employee);
        _context.SaveChanges();

        Console.WriteLine($"Employee {id} deleted");
        return NoContent();
    }
}
