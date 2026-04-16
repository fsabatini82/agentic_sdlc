namespace EmployeePortal.Models;

public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }  // Denormalized, should be navigation property
    public string Type { get; set; }          // "VACATION", "SICK", "PERSONAL" - magic strings!
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }        // "PENDING", "APPROVED", "REJECTED" - not enum!
    public string Reason { get; set; }
    public string ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public int DaysRequested { get; set; }
}
