using My.App.Interfaces;
using System.Text;

namespace My.App.Commands
{
    public class ManagerInfoCommand : IExecutable
    {
        private readonly IManagerController managerController;

        public ManagerInfoCommand(IManagerController managerController)
        {
            this.managerController = managerController;
        }

        public string Execute(string[] args)
        {
            int managerId = int.Parse(args[0]);

            var managerDto = this.managerController.GetManagerInfo(managerId);

            var sb = new StringBuilder();
            sb.AppendLine($"{managerDto.FirstName} {managerDto.LastName} | Employees: {managerDto.SubordinatesCount}");

            foreach (var sub in managerDto.SubordinateDtos)
            {
                sb.AppendLine($"\t- {sub.FirstName} {sub.LastName} - ${sub.Salary:f2}");
            }

            return sb.ToString().Trim();
        }
    }
}
