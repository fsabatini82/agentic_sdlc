namespace EmployeePortal.Models;

public class Skill
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Level { get; set; }  // "BEGINNER", "INTERMEDIATE", "EXPERT" - magic strings!
}
