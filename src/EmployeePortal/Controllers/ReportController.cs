using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeePortal.Data;
using EmployeePortal.Models;

namespace EmployeePortal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly PortalDbContext _context;

    // Code smell: no ILogger
    public ReportController(PortalDbContext context)
    {
        _context = context;
    }

    // ========================================================================
    // GET api/report/salary?department=Engineering
    // CRITICAL: SQL INJECTION via FromSqlRaw + string interpolation!
    // ========================================================================
    [HttpGet("salary")]
    public ActionResult GetSalaryReport([FromQuery] string department)
    {
        Console.WriteLine("GetSalaryReport called for department: " + department);

        if (string.IsNullOrEmpty(department))
            return BadRequest("Department parameter is required");

        // CRITICAL: SQL injection vulnerability!
        // An attacker can pass: department = "'; DROP TABLE Employees; --"
        var employees = _context.Employees
            .FromSqlRaw($"SELECT * FROM Employees WHERE Department = '{department}'")
            .ToList(); // Also: no async!

        var report = new
        {
            department,
            employeeCount = employees.Count,
            totalSalary = employees.Sum(e => e.Salary),
            averageSalary = employees.Any() ? employees.Average(e => e.Salary) : 0,
            minSalary = employees.Any() ? employees.Min(e => e.Salary) : 0,
            maxSalary = employees.Any() ? employees.Max(e => e.Salary) : 0,
            employees = employees.Select(e => new
            {
                e.Id,
                Name = $"{e.FirstName} {e.LastName}",
                e.Position,
                e.Salary // PII: salary exposed in report without auth!
            })
        };

        Console.WriteLine($"Salary report generated: {employees.Count} employees");
        return Ok(report);
    }

    // ========================================================================
    // GET api/report/headcount
    // This one is OK — uses LINQ properly
    // ========================================================================
    [HttpGet("headcount")]
    public ActionResult GetHeadcountReport()
    {
        Console.WriteLine("GetHeadcountReport called");

        var report = _context.Employees
            .Where(e => e.IsActive)
            .GroupBy(e => e.Department)
            .Select(g => new
            {
                Department = g.Key,
                Count = g.Count(),
                AverageSalary = g.Average(e => e.Salary)
            })
            .ToList(); // No async, but at least no SQL injection

        return Ok(report);
    }

    // ========================================================================
    // GET api/report/leave?status=APPROVED
    // CRITICAL: Second SQL injection!
    // ========================================================================
    [HttpGet("leave")]
    public ActionResult GetLeaveReport([FromQuery] string status = "APPROVED")
    {
        Console.WriteLine("GetLeaveReport called for status: " + status);

        // CRITICAL: SQL injection vulnerability — SECOND instance!
        var requests = _context.LeaveRequests
            .FromSqlRaw($"SELECT * FROM LeaveRequests WHERE Status = '{status}'")
            .ToList();

        var report = new
        {
            status,
            totalRequests = requests.Count,
            totalDays = requests.Sum(r => r.DaysRequested),
            byType = requests
                .GroupBy(r => r.Type)
                .Select(g => new { Type = g.Key, Count = g.Count(), Days = g.Sum(r => r.DaysRequested) }),
            byEmployee = requests
                .GroupBy(r => r.EmployeeName)
                .Select(g => new { Employee = g.Key, Count = g.Count(), Days = g.Sum(r => r.DaysRequested) })
        };

        Console.WriteLine($"Leave report generated: {requests.Count} requests");
        return Ok(report);
    }
}
