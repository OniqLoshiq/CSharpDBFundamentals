using My.App.Interfaces;
using System.Text;

namespace My.App.Commands
{
    public class EmployeePersonalInfoCommand : IExecutable
    {
        private readonly IEmployeeController employeeController;

        public EmployeePersonalInfoCommand(IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        public string Execute(string[] args)
        {
            int id = int.Parse(args[0]);

            var employeeDto = this.employeeController.GetEmployeePersonalInfo(id);

            var sb = new StringBuilder();

            sb.AppendLine($"ID: {employeeDto.EmployeeId} - {employeeDto.FirstName} {employeeDto.LastName} - ${employeeDto.Salary:f2}")
              .AppendLine($"Birthday: {employeeDto.Birthday?.ToString("dd-MM-yyyy")}")
              .AppendLine($"Address: {employeeDto.Address}");

            return sb.ToString().Trim();
        }
    }
}
