using P02_DatabaseFirst.Data;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace _11_FindLatest10Projs
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SoftUniContext())
            {
                var projs = ctx.Projects.OrderByDescending(p => p.StartDate)
                                        .Select(p => new
                                        {
                                            p.Name,
                                            p.Description,
                                            p.StartDate
                                        })
                                        .Take(10)
                                        .OrderBy(p => p.Name)
                                        .ToArray();

                using (StreamWriter sw = new StreamWriter("../../../../11_FindLatest10Projs.txt"))
                {
                    foreach (var p in projs)
                    {
                        sw.WriteLine($"{p.Name}" +
                                     $"{Environment.NewLine}" +
                                     $"{p.Description}" +
                                     $"{Environment.NewLine}" +
                                     $"{p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
                    }
                }
            }
        }
    }
}
