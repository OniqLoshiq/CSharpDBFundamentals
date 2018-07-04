using MiniORM.App.Data;
using MiniORM.App.Data.Entities;
using System.Linq;

namespace MiniORM.App
{
    public class Program
    {
        const string connectionString = @"Server=.;Database=MiniORM;Integrated Security=True";

        public static void Main(string[] args)
        {
            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Gopeto",
                LastName = "Inserted17",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true
            });

            

            var employee = context.Employees.Last();
            employee.FirstName = "Modified17";

            context.SaveChanges();
        }
    }
}