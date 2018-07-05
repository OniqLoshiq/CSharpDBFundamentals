using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace _04_EmplWithSalaryOver50k
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SoftUniContext())
            {
                var employees = ctx.Employees
                                .Where(e => e.Salary > 50_000)
                                .OrderBy(e => e.FirstName)
                                .Select(e => e.FirstName)
                                .ToArray();

                using (StreamWriter sw = new StreamWriter("../../../../04_EmplWithSalaryOver50k.txt"))
                {
                    Array.ForEach(employees, e => sw.WriteLine($"{e}"));
                }
            }
        }
    }
}
