using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System;
using System.IO;
using System.Linq;

namespace _06_AddAddressUpdEmpl
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SoftUniContext())
            {
                var address = new Address()
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4
                };

                var employee = ctx.Employees
                               .FirstOrDefault(e => e.LastName == "Nakov");

                employee.Address = address;
                ctx.SaveChanges();

                var employees = ctx.Employees
                                .OrderByDescending(e => e.AddressId)
                                .Select(e => e.Address.AddressText)
                                .Take(10)
                                .ToArray();

                using (StreamWriter sw = new StreamWriter("../../../../06_AddAddressUpdEmpl.txt"))
                {
                    Array.ForEach(employees, e => sw.WriteLine(e));
                }
            }
        }
    }
}
