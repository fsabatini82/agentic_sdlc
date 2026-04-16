using EmployeePortal.Models;

namespace EmployeePortal.Data;

public static class SeedData
{
    public static void Initialize(PortalDbContext context)
    {
        // Don't seed if data already exists
        if (context.Departments.Any())
            return;

        // Departments
        var departments = new Department[]
        {
            new Department { Id = 1, Name = "Engineering", Code = "ENG", ManagerName = "Marco Rossi", MaxHeadcount = 20 },
            new Department { Id = 2, Name = "Human Resources", Code = "HR", ManagerName = "Giulia Bianchi", MaxHeadcount = 8 },
            new Department { Id = 3, Name = "Finance", Code = "FIN", ManagerName = "Alessandro Moretti", MaxHeadcount = 10 },
            new Department { Id = 4, Name = "Marketing", Code = "MKT", ManagerName = "Francesca Colombo", MaxHeadcount = 12 },
            new Department { Id = 5, Name = "Operations", Code = "OPS", ManagerName = "Luca Ricci", MaxHeadcount = 15 }
        };
        context.Departments.AddRange(departments);
        context.SaveChanges();

        // Employees
        var employees = new Employee[]
        {
            new Employee { Id = 1, FirstName = "Marco", LastName = "Rossi", Email = "marco.rossi@company.com", Salary = 78000m, Department = "Engineering", HireDate = new DateTime(2019, 3, 15), Position = "Senior Developer", IsActive = true },
            new Employee { Id = 2, FirstName = "Giulia", LastName = "Bianchi", Email = "giulia.bianchi@company.com", Salary = 72000m, Department = "Human Resources", HireDate = new DateTime(2018, 7, 1), Position = "HR Manager", IsActive = true },
            new Employee { Id = 3, FirstName = "Alessandro", LastName = "Moretti", Email = "alessandro.moretti@company.com", Salary = 85000m, Department = "Finance", HireDate = new DateTime(2017, 1, 10), Position = "Finance Director", IsActive = true },
            new Employee { Id = 4, FirstName = "Francesca", LastName = "Colombo", Email = "francesca.colombo@company.com", Salary = 68000m, Department = "Marketing", HireDate = new DateTime(2020, 5, 20), Position = "Marketing Lead", IsActive = true },
            new Employee { Id = 5, FirstName = "Luca", LastName = "Ricci", Email = "luca.ricci@company.com", Salary = 75000m, Department = "Operations", HireDate = new DateTime(2018, 11, 3), Position = "Operations Manager", IsActive = true },
            new Employee { Id = 6, FirstName = "Sara", LastName = "Ferrari", Email = "sara.ferrari@company.com", Salary = 52000m, Department = "Engineering", HireDate = new DateTime(2021, 9, 1), Position = "Junior Developer", IsActive = true },
            new Employee { Id = 7, FirstName = "Davide", LastName = "Russo", Email = "davide.russo@company.com", Salary = 62000m, Department = "Engineering", HireDate = new DateTime(2020, 2, 14), Position = "Mid Developer", IsActive = true },
            new Employee { Id = 8, FirstName = "Elena", LastName = "Conti", Email = "elena.conti@company.com", Salary = 48000m, Department = "Human Resources", HireDate = new DateTime(2022, 6, 15), Position = "HR Specialist", IsActive = true },
            new Employee { Id = 9, FirstName = "Matteo", LastName = "Gallo", Email = "matteo.gallo@company.com", Salary = 58000m, Department = "Finance", HireDate = new DateTime(2021, 3, 22), Position = "Accountant", IsActive = true },
            new Employee { Id = 10, FirstName = "Chiara", LastName = "Lombardi", Email = "chiara.lombardi@company.com", Salary = 45000m, Department = "Marketing", HireDate = new DateTime(2023, 1, 9), Position = "Content Specialist", IsActive = true },
            new Employee { Id = 11, FirstName = "Roberto", LastName = "Mancini", Email = "roberto.mancini@company.com", Salary = 70000m, Department = "Engineering", HireDate = new DateTime(2019, 8, 5), Position = "DevOps Engineer", IsActive = true },
            new Employee { Id = 12, FirstName = "Valentina", LastName = "Greco", Email = "valentina.greco@company.com", Salary = 55000m, Department = "Operations", HireDate = new DateTime(2022, 4, 18), Position = "Operations Analyst", IsActive = true },
            new Employee { Id = 13, FirstName = "Andrea", LastName = "Barbieri", Email = "andrea.barbieri@company.com", Salary = 38000m, Department = "Engineering", HireDate = new DateTime(2024, 2, 1), Position = "Intern Developer", IsActive = true },
            new Employee { Id = 14, FirstName = "Federica", LastName = "Marchetti", Email = "federica.marchetti@company.com", Salary = 60000m, Department = "Finance", HireDate = new DateTime(2020, 10, 12), Position = "Financial Analyst", IsActive = true },
            new Employee { Id = 15, FirstName = "Simone", LastName = "De Luca", Email = "simone.deluca@company.com", Salary = 35000m, Department = "Marketing", HireDate = new DateTime(2024, 6, 1), Position = "Marketing Assistant", IsActive = false }
        };
        context.Employees.AddRange(employees);
        context.SaveChanges();

        // Leave Requests
        var leaveRequests = new LeaveRequest[]
        {
            new LeaveRequest { Id = 1, EmployeeId = 1, EmployeeName = "Marco Rossi", Type = "VACATION", StartDate = new DateTime(2026, 5, 1), EndDate = new DateTime(2026, 5, 10), Status = "APPROVED", Reason = "Family holiday", ApprovedBy = "Giulia Bianchi", ApprovedAt = new DateTime(2026, 4, 5), DaysRequested = 8 },
            new LeaveRequest { Id = 2, EmployeeId = 6, EmployeeName = "Sara Ferrari", Type = "SICK", StartDate = new DateTime(2026, 4, 8), EndDate = new DateTime(2026, 4, 10), Status = "APPROVED", Reason = "Flu", ApprovedBy = "Marco Rossi", ApprovedAt = new DateTime(2026, 4, 8), DaysRequested = 3 },
            new LeaveRequest { Id = 3, EmployeeId = 3, EmployeeName = "Alessandro Moretti", Type = "PERSONAL", StartDate = new DateTime(2026, 4, 20), EndDate = new DateTime(2026, 4, 22), Status = "PENDING", Reason = "Personal matters", DaysRequested = 3 },
            new LeaveRequest { Id = 4, EmployeeId = 7, EmployeeName = "Davide Russo", Type = "VACATION", StartDate = new DateTime(2026, 6, 15), EndDate = new DateTime(2026, 6, 30), Status = "PENDING", Reason = "Summer vacation", DaysRequested = 12 },
            new LeaveRequest { Id = 5, EmployeeId = 2, EmployeeName = "Giulia Bianchi", Type = "VACATION", StartDate = new DateTime(2026, 7, 1), EndDate = new DateTime(2026, 7, 14), Status = "PENDING", Reason = "Annual leave", DaysRequested = 10 },
            new LeaveRequest { Id = 6, EmployeeId = 10, EmployeeName = "Chiara Lombardi", Type = "SICK", StartDate = new DateTime(2026, 3, 25), EndDate = new DateTime(2026, 3, 26), Status = "APPROVED", Reason = "Doctor appointment", ApprovedBy = "Francesca Colombo", ApprovedAt = new DateTime(2026, 3, 25), DaysRequested = 2 },
            new LeaveRequest { Id = 7, EmployeeId = 11, EmployeeName = "Roberto Mancini", Type = "VACATION", StartDate = new DateTime(2026, 8, 1), EndDate = new DateTime(2026, 8, 5), Status = "REJECTED", Reason = "Short trip", DaysRequested = 5 },
            new LeaveRequest { Id = 8, EmployeeId = 4, EmployeeName = "Francesca Colombo", Type = "PERSONAL", StartDate = new DateTime(2026, 4, 15), EndDate = new DateTime(2026, 4, 15), Status = "APPROVED", Reason = "Moving day", ApprovedBy = "Luca Ricci", ApprovedAt = new DateTime(2026, 4, 10), DaysRequested = 1 }
        };
        context.LeaveRequests.AddRange(leaveRequests);
        context.SaveChanges();

        // Skills
        var skills = new Skill[]
        {
            new Skill { Id = 1, EmployeeId = 1, Name = "C#", Level = "EXPERT" },
            new Skill { Id = 2, EmployeeId = 1, Name = "Azure", Level = "INTERMEDIATE" },
            new Skill { Id = 3, EmployeeId = 6, Name = "C#", Level = "BEGINNER" },
            new Skill { Id = 4, EmployeeId = 7, Name = "JavaScript", Level = "INTERMEDIATE" },
            new Skill { Id = 5, EmployeeId = 7, Name = "React", Level = "INTERMEDIATE" },
            new Skill { Id = 6, EmployeeId = 11, Name = "Docker", Level = "EXPERT" },
            new Skill { Id = 7, EmployeeId = 11, Name = "Kubernetes", Level = "INTERMEDIATE" },
            new Skill { Id = 8, EmployeeId = 13, Name = "Python", Level = "BEGINNER" },
            new Skill { Id = 9, EmployeeId = 3, Name = "Excel", Level = "EXPERT" },
            new Skill { Id = 10, EmployeeId = 14, Name = "Power BI", Level = "INTERMEDIATE" }
        };
        context.Skills.AddRange(skills);
        context.SaveChanges();
    }
}
