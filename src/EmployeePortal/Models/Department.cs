namespace EmployeePortal.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }        // e.g., "ENG", "HR", "FIN"
    public string ManagerName { get; set; }  // String, not FK to Employee!
    public int MaxHeadcount { get; set; }
}
