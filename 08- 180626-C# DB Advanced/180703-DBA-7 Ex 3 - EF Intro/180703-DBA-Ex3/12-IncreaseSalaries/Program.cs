using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace _12_IncreaseSalaries
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SoftUniContext())
            {
                var employees = ctx.Employees
                                        .Where(e => e.Department.Name == "Engineering" 
                                                 || e.Department.Name == "Tool Design" 
                                                 || e.Department.Name == "Marketing"
                                                 || e.Department.Name == "Information Services")
                                        .ToList();

                employees.ForEach(e => e.Salary *= 1.12m);
                ctx.SaveChanges();

                var employeesToPrint = employees.OrderBy(e => e.FirstName)
                                                .ThenBy(e => e.LastName)
                                                .Select(e => new
                                                             {
                                                                 e.FirstName,
                                                                 e.LastName,
                                                                 e.Salary
                                                             })
                                                 .ToArray();

                using (StreamWriter sw = new StreamWriter("../../../../12_IncreaseSalaries.txt"))
                {
                    foreach (var e in employeesToPrint)
                    {
                        sw.WriteLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
                    }
                }
            }
        }
    }
}
