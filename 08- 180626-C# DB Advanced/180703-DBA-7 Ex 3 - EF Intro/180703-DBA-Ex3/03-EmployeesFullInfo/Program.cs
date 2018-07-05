using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace _03_EmployeesFullInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SoftUniContext())
            {
                var employees = ctx.Employees
                                .OrderBy(e => e.EmployeeId)
                                .Select(e => new
                                {
                                    e.FirstName,
                                    e.LastName,
                                    e.MiddleName,
                                    e.JobTitle,
                                    e.Salary
                                })
                                .ToArray();


                using (StreamWriter sw = new StreamWriter("../../../../03_EmplFullInfo.txt"))
                {
                    Array.ForEach(employees, e => sw.WriteLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}"));
                }
            }
        }
    }
}
