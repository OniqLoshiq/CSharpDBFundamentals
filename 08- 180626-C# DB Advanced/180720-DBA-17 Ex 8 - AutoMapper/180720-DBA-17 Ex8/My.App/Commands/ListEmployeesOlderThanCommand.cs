using My.App.Interfaces;
using System.Text;

namespace My.App.Commands
{
    public class ListEmployeesOlderThanCommand : IExecutable
    {
        private readonly IEmployeeController employeeController;

        public ListEmployeesOlderThanCommand(IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        public string Execute(string[] args)
        {
            int age = int.Parse(args[0]);

            var employees = this.employeeController.GetEmployeesOlderThan(age);

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - ${e.Salary:f2} - Manager: {e.ManagerFullName ?? "[no manager]"}");
            }

            return sb.ToString().Trim();
        }
    }
}
