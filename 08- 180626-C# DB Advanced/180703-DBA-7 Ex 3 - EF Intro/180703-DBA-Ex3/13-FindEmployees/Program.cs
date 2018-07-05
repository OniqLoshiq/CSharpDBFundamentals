using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace _13_FindEmployees
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SoftUniContext())
            {
                var employees = ctx.Employees.Where(e => EF.Functions.Like(e.FirstName,"Sa%"))
                                             .Select(e => new
                                             {
                                                 e.FirstName,
                                                 e.LastName,
                                                 e.JobTitle,
                                                 e.Salary
                                             })
                                             .OrderBy(e => e.FirstName)
                                             .ThenBy(e => e.LastName)
                                             .ToArray();

                using (StreamWriter sw = new StreamWriter("../../../../13_FindEmployees.txt"))
                {
                    foreach (var e in employees)
                    {
                        sw.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
                    }
                }
            }
        }
    }
}
