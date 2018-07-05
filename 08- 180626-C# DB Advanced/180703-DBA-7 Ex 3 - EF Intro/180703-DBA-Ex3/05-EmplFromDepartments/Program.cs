using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace _05_EmplFromDepartments
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SoftUniContext())
            {
                var employees = ctx.Employees
                                .Where(e => e.Department.Name == "Research and Development")
                                .OrderBy(e => e.Salary)
                                .ThenByDescending(e => e.FirstName)
                                .Select(e => new
                                {
                                    e.FirstName,
                                    e.LastName,
                                    DepartmentName = e.Department.Name,
                                    e.Salary
                                })
                                .ToArray();

                using (StreamWriter sw = new StreamWriter("../../../../05_EmplFromDepartment.txt"))
                {
                    Array.ForEach(employees, e => sw.WriteLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}"));
                }
            }
        }
    }
}
