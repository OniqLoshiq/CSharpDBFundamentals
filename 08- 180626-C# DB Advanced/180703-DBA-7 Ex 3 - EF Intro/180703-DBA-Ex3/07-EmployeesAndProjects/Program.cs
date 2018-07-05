using P02_DatabaseFirst.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace _07_EmployeesAndProjects
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SoftUniContext())
            {
                var employees = ctx.Employees
                                .Where(e => e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                                .Select(e => new
                                {
                                    FullName = e.FirstName + " " + e.LastName,
                                    ManagerName = e.Manager.FirstName + " " + e.Manager.LastName,
                                    Projects = e.EmployeesProjects.Select(p => new
                                    {
                                        p.Project.Name,
                                        p.Project.StartDate,
                                        p.Project.EndDate
                                    }).ToArray()
                                })
                                .Take(30)
                                .ToArray();

                using (StreamWriter sw = new StreamWriter("../../../../07_EmployeesAndProjects.txt"))
                {
                    foreach (var e in employees)
                    {
                        sw.WriteLine($"{e.FullName} - Manager: {e.ManagerName}");

                        foreach (var p in e.Projects)
                        {
                            sw.WriteLine($"--{p.Name} - {p.StartDate.ToString(@"M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - " +
                                $"{p.EndDate?.ToString(@"M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) ?? "not finished"}");
                        }
                    }
                }
            }
        }
    }
}
