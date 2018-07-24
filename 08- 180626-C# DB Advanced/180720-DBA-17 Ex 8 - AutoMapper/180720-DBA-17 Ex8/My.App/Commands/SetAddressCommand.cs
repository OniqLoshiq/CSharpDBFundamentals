using My.App.Interfaces;
using System.Linq;

namespace My.App.Commands
{
    public class SetAddressCommand : IExecutable
    {
        private readonly IEmployeeController employeeController;

        public SetAddressCommand(IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        public string Execute(string[] args)
        {
            int id = int.Parse(args[0]);
            string address = string.Join(" ", args.Skip(1));

            this.employeeController.SetAddress(id, address);

            return $"Successfully added address for employee with id {id}";
        }
    }
}
