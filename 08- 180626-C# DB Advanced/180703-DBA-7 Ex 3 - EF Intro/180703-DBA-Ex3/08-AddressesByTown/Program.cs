using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace _08_AddressesByTown
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SoftUniContext())
            {
                var addresses = ctx.Addresses
                                .OrderByDescending(a => a.Employees.Count)
                                .ThenBy(a => a.Town.Name)
                                .ThenBy(a => a.AddressText)
                                .Select(a => new
                                {
                                    Address = a.AddressText,
                                    TownName = a.Town.Name,
                                    EmployeesCount = a.Employees.Count
                                })
                                .Take(10)
                                .ToArray();

                using (StreamWriter sw = new StreamWriter("../../../../08_AddressesToTown.txt"))
                {
                    foreach (var a in addresses)
                    {
                        sw.WriteLine($"{a.Address}, {a.TownName} - {a.EmployeesCount} employees");
                    }
                }
            }
        }
    }
}
