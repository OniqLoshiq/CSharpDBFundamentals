using My.App.Interfaces;
using System;
using System.Globalization;

namespace My.App.Commands
{
    public class SetBirthdayCommand : IExecutable
    {
        private readonly IEmployeeController employeeController;

        public SetBirthdayCommand(string[] args, IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        public string Execute(string[] args)
        {
            int id = int.Parse(args[0]);
            DateTime date = DateTime.ParseExact(args[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            this.employeeController.SetBirthday(id, date);

            return $"Successfully added birthday for employee with id {id}";
        }
    }
}
