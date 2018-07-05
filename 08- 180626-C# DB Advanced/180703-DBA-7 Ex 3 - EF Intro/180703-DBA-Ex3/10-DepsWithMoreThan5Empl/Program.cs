using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace _10_DepsWithMoreThan5Empl
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SoftUniContext())
            {
                var deps = ctx.Departments.Include(d => d.Employees)
                                          .Where(d => d.Employees.Count > 5)
                                          .OrderBy(d => d.Employees.Count)
                                          .ThenBy(d => d.Name)
                                          .Select(d => new
                                          {
                                              DepartmentName = d.Name,
                                              DepartmentManager = d.Manager.FirstName + " " + d.Manager.LastName,
                                              Employees = d.Employees.Select(e => new
                                                                     {
                                                                         e.FirstName,
                                                                         e.LastName,
                                                                         e.JobTitle
                                                                     })
                                                                     .OrderBy(e => e.FirstName)
                                                                     .ThenBy(e => e.LastName)
                                          })
                                          .ToArray();
                
                using (StreamWriter sw = new StreamWriter("../../../../10_DepsWithMoreThan5Empl.txt"))
                {
                    foreach (var d in deps)
                    {
                        sw.WriteLine($"{d.DepartmentName} - {d.DepartmentManager}");

                        foreach (var e in d.Employees)
                        {
                            sw.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                        }

                        sw.WriteLine("----------");
                    }
                }
            }
        }
    }
}
