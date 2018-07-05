using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using System.IO;
using System.Linq;

namespace _09_Employee147
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SoftUniContext())
            {
                var employee = ctx.Employees.Include(e => e.EmployeesProjects)
                               .ThenInclude(ep => ep.Project)
                               .SingleOrDefault(e => e.EmployeeId == 147);
                
                using (StreamWriter sw = new StreamWriter("../../../../09_Employee147.txt"))
                {
                    sw.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

                    foreach (var p in employee.EmployeesProjects.OrderBy(x => x.Project.Name))
                    {
                        sw.WriteLine($"{p.Project.Name}");
                    }
                }
            }
        }
    }
}
