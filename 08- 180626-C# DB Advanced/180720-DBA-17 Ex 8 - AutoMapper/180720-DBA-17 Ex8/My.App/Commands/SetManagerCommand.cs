using My.App.Interfaces;

namespace My.App.Commands
{
    public class SetManagerCommand : IExecutable
    {
        private readonly IManagerController managerController;

        public SetManagerCommand(IManagerController managerController)
        {
            this.managerController = managerController;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);
            int managerId = int.Parse(args[1]);

            this.managerController.SetManager(employeeId, managerId);

            return $"Successfully added manager to employee with id {employeeId}";
        }
    }
}
