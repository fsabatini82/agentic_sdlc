using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeePortal.Data;
using EmployeePortal.Models;

namespace EmployeePortal.Controllers;

[ApiController]
[Route("api/leave-requests")]
public class LeaveRequestController : ControllerBase
{
    private readonly PortalDbContext _context;

    // Code smell: no ILogger injected, uses Console.WriteLine
    public LeaveRequestController(PortalDbContext context)
    {
        _context = context;
    }

    // ========================================================================
    // GET api/leave-requests
    // Problem: no pagination, synchronous
    // ========================================================================
    [HttpGet]
    public ActionResult<IEnumerable<LeaveRequest>> GetAll()
    {
        Console.WriteLine("GetAll leave requests called at " + DateTime.Now);
        var requests = _context.LeaveRequests.ToList(); // No async!
        return Ok(requests);
    }

    // ========================================================================
    // GET api/leave-requests/pending
    // Problem: duplicated filtering logic (copy-paste from GetAll)
    // ========================================================================
    [HttpGet("pending")]
    public ActionResult<IEnumerable<LeaveRequest>> GetPending()
    {
        Console.WriteLine("GetPending called");
        // Duplicated: same pattern as GetAll but with filter
        var requests = _context.LeaveRequests.ToList(); // Load ALL then filter in memory!
        var pending = requests.Where(r => r.Status == "PENDING").ToList();
        return Ok(pending);
    }

    // ========================================================================
    // GET api/leave-requests/employee/{employeeId}
    // Problem: duplicated logic AGAIN
    // ========================================================================
    [HttpGet("employee/{employeeId}")]
    public ActionResult<IEnumerable<LeaveRequest>> GetByEmployee(int employeeId)
    {
        Console.WriteLine("GetByEmployee called for: " + employeeId);
        // Duplicated: loads everything, filters in memory
        var requests = _context.LeaveRequests.ToList();
        var filtered = requests.Where(r => r.EmployeeId == employeeId).ToList();
        return Ok(filtered);
    }

    // ========================================================================
    // POST api/leave-requests
    // Problem: no validation at all
    // ========================================================================
    [HttpPost]
    public ActionResult<LeaveRequest> Create([FromBody] LeaveRequest request)
    {
        Console.WriteLine("Creating leave request at " + DateTime.Now);

        // No validation! EndDate can be before StartDate, days can be negative,
        // Status can be "BANANA", Type can be anything
        request.Status = "PENDING";
        request.DaysRequested = (request.EndDate - request.StartDate).Days + 1;

        _context.LeaveRequests.Add(request);
        _context.SaveChanges(); // No async!

        Console.WriteLine($"Leave request {request.Id} created for employee {request.EmployeeId}");

        return CreatedAtAction(nameof(GetAll), new { id = request.Id }, request);
    }

    // ========================================================================
    // POST api/leave-requests/{id}/approve
    // THE GOD METHOD — ~90 lines of inline everything
    // Should be: validator + service + notification service + event
    // ========================================================================
    [HttpPost("{id}/approve")]
    public ActionResult ApproveLeave(int id, [FromQuery] string approvedBy = "Manager")
    {
        Console.WriteLine($"ApproveLeave called for request {id} by {approvedBy}");

        // --- Step 1: Find the request ---
        var request = _context.LeaveRequests.Find(id);
        if (request == null)
        {
            Console.WriteLine("Leave request not found: " + id);
            return NotFound("Leave request not found");
        }

        // --- Step 2: Check if already approved (but doesn't return early!) ---
        if (request.Status == "APPROVED")
        {
            Console.WriteLine("WARNING: Request already approved, but continuing anyway");
            // Bug: should return early, but just logs and continues!
        }

        if (request.Status == "REJECTED")
        {
            Console.WriteLine("WARNING: Trying to approve a rejected request");
            // Bug: allows approving a rejected request!
        }

        // --- Step 3: Check leave balance (hardcoded 25 days/year) ---
        var yearStart = new DateTime(DateTime.UtcNow.Year, 1, 1);
        var yearEnd = new DateTime(DateTime.UtcNow.Year, 12, 31);
        var usedDays = _context.LeaveRequests
            .Where(r => r.EmployeeId == request.EmployeeId
                        && r.Status == "APPROVED"
                        && r.StartDate >= yearStart
                        && r.EndDate <= yearEnd)
            .ToList() // Loads all to memory!
            .Sum(r => r.DaysRequested);

        var maxDays = 25; // Hardcoded! Should be configurable per employee/contract
        var remaining = maxDays - usedDays;

        Console.WriteLine($"Employee {request.EmployeeId}: used {usedDays}/{maxDays} days, remaining: {remaining}");

        if (request.DaysRequested > remaining)
        {
            Console.WriteLine("REJECTED: Not enough leave days");
            return BadRequest($"Not enough leave days. Requested: {request.DaysRequested}, Available: {remaining}");
        }

        // --- Step 4: Check for date conflicts (duplicated logic!) ---
        var conflicts = _context.LeaveRequests
            .Where(r => r.EmployeeId == request.EmployeeId
                        && r.Status == "APPROVED"
                        && r.Id != request.Id)
            .ToList() // Loads all to memory AGAIN!
            .Where(r => request.StartDate <= r.EndDate && request.EndDate >= r.StartDate)
            .ToList();

        if (conflicts.Any())
        {
            Console.WriteLine($"CONFLICT: {conflicts.Count} overlapping leave requests found");
            return BadRequest($"Date conflict with {conflicts.Count} existing approved leave(s)");
        }

        // --- Step 5: Update status ---
        request.Status = "APPROVED";
        request.ApprovedBy = approvedBy;
        request.ApprovedAt = DateTime.UtcNow;

        _context.SaveChanges(); // First save

        // --- Step 6: Inline notification (should be a separate service!) ---
        Console.WriteLine($"NOTIFICATION: Leave request {id} approved for employee {request.EmployeeId}");
        Console.WriteLine($"NOTIFICATION: Sending email to {request.EmployeeName}...");
        Console.WriteLine($"NOTIFICATION: Sending email to manager {approvedBy}...");

        // Simulate email sending
        Thread.Sleep(1000); // Code smell: blocking the thread!

        // --- Step 7: Inline statistics update (should be event-driven) ---
        var totalPending = _context.LeaveRequests.Count(r => r.Status == "PENDING");
        var totalApproved = _context.LeaveRequests.Count(r => r.Status == "APPROVED");
        var totalRejected = _context.LeaveRequests.Count(r => r.Status == "REJECTED");

        Console.WriteLine($"STATS: Pending={totalPending}, Approved={totalApproved}, Rejected={totalRejected}");

        // --- Step 8: Update audit trail (second save!) ---
        var employee = _context.Employees.Find(request.EmployeeId);
        if (employee != null)
        {
            employee.AuditTrail += $"; Leave {id} approved at {DateTime.UtcNow}";
            employee.LastModifiedBy = approvedBy;
        }

        _context.SaveChanges(); // Second save — wasteful!

        Console.WriteLine($"AUDIT: Leave request {id} approved successfully");

        return Ok(new { message = "Leave approved", requestId = id, daysUsed = usedDays + request.DaysRequested, daysRemaining = remaining - request.DaysRequested });
    }

    // ========================================================================
    // POST api/leave-requests/{id}/reject
    // Problem: similar pattern, Thread.Sleep, no validation
    // ========================================================================
    [HttpPost("{id}/reject")]
    public ActionResult RejectLeave(int id, [FromQuery] string rejectedBy = "Manager", [FromQuery] string reason = "")
    {
        Console.WriteLine($"RejectLeave called for request {id}");

        var request = _context.LeaveRequests.Find(id);
        if (request == null)
            return NotFound();

        // No status check — can reject an already approved request!
        request.Status = "REJECTED";
        request.ApprovedBy = rejectedBy; // Misleading field name for rejection
        request.ApprovedAt = DateTime.UtcNow;

        _context.SaveChanges();

        Console.WriteLine($"NOTIFICATION: Leave {id} rejected. Reason: {reason}");
        Thread.Sleep(500); // Another blocking sleep

        return Ok(new { message = "Leave rejected", requestId = id });
    }

    // ========================================================================
    // GET api/leave-requests/conflicts/{employeeId}
    // Problem: duplicated conflict detection (same logic as ApproveLeave)
    // ========================================================================
    [HttpGet("conflicts/{employeeId}")]
    public ActionResult GetConflicts(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        Console.WriteLine($"Checking conflicts for employee {employeeId}");

        // DUPLICATED: exact same conflict logic as in ApproveLeave
        var conflicts = _context.LeaveRequests
            .Where(r => r.EmployeeId == employeeId && r.Status == "APPROVED")
            .ToList()
            .Where(r => startDate <= r.EndDate && endDate >= r.StartDate)
            .ToList();

        return Ok(new { hasConflicts = conflicts.Any(), count = conflicts.Count, conflicts });
    }

    // ========================================================================
    // GET api/leave-requests/balance/{employeeId}
    // Problem: hardcoded 25 days, duplicated calculation
    // ========================================================================
    [HttpGet("balance/{employeeId}")]
    public ActionResult GetLeaveBalance(int employeeId)
    {
        Console.WriteLine($"GetLeaveBalance for employee {employeeId}");

        // DUPLICATED: same calculation as in ApproveLeave
        var yearStart = new DateTime(DateTime.UtcNow.Year, 1, 1);
        var yearEnd = new DateTime(DateTime.UtcNow.Year, 12, 31);
        var usedDays = _context.LeaveRequests
            .Where(r => r.EmployeeId == employeeId
                        && r.Status == "APPROVED"
                        && r.StartDate >= yearStart
                        && r.EndDate <= yearEnd)
            .ToList()
            .Sum(r => r.DaysRequested);

        var maxDays = 25; // Hardcoded AGAIN!
        var remaining = maxDays - usedDays;

        // Count by type (more duplication)
        var allRequests = _context.LeaveRequests
            .Where(r => r.EmployeeId == employeeId && r.StartDate >= yearStart)
            .ToList();

        var byType = allRequests
            .GroupBy(r => r.Type)
            .Select(g => new { Type = g.Key, Count = g.Count(), Days = g.Sum(r => r.DaysRequested) })
            .ToList();

        return Ok(new
        {
            employeeId,
            year = DateTime.UtcNow.Year,
            totalAllowance = maxDays,
            used = usedDays,
            remaining,
            byType,
            pending = allRequests.Count(r => r.Status == "PENDING")
        });
    }
}
