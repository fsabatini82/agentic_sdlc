namespace EmployeePortal.Models;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }    // No [Required]
    public string LastName { get; set; }
    public string Email { get; set; }        // No email format validation
    public decimal Salary { get; set; }      // No range validation, PII exposed!
    public string Department { get; set; }   // String, not FK relationship!
    public DateTime HireDate { get; set; }
    public string Position { get; set; }
    public bool IsActive { get; set; } = true;

    // Code smell: public fields mixed with properties
    public string AuditTrail;
    public string LastModifiedBy;
}
