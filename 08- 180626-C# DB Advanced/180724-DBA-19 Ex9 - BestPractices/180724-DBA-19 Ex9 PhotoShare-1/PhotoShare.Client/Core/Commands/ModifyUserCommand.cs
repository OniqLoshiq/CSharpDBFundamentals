namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Services.Contracts;

    public class ModifyUserCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly ITownService townService;

        public ModifyUserCommand(IUserService userService, ITownService townService)
        {
            this.userService = userService;
            this.townService = townService;
        }

        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public string Execute(string[] data)
        {
            string cmdName = this.GetType().Name;
            int cmdIndex = cmdName.IndexOf("Command");
            cmdName = cmdName.Substring(0, cmdIndex);

            if (data.Length != 3)
            {
                throw new InvalidOperationException($"Command {cmdName} not valid!");
            }
            
            string username = data[0];
            string property = data[1].ToLower();
            string value = data[2];

            var userExists = this.userService.Exists(username);

            if(!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            var userId = this.userService.ByUsername<UserDto>(username).Id;

            if(property == "password")
            {
                SetPassword(userId, value);
            }
            else if(property == "borntown")
            {
                SetBornTown(userId, value);
            }
            else if(property == "setcurrenttown")
            {
                SetCurrentTown(userId, value);
            }
            else
            {
                throw new ArgumentException($"Property {property} not supported!");
            }
            
            return $"User {username} {property} is {value}";
        }

        private void SetCurrentTown(int userId, string value)
        {
            var townExists = this.townService.Exists(value);

            if (!townExists)
            {
                throw new ArgumentException($"Value {value} not valid\nTown not found!");
            }

            var townId = this.townService.ByName<TownDto>(value).Id;

            this.userService.SetCurrentTown(userId, townId);
        }

        private void SetBornTown(int userId, string value)
        {
            var townExists = this.townService.Exists(value);

            if(!townExists)
            {
                throw new ArgumentException($"Value {value} not valid\nTown not found!");
            }

            var townId = this.townService.ByName<TownDto>(value).Id;

            this.userService.SetBornTown(userId, townId);
        }

        private void SetPassword(int userId, string value)
        {
            var isLower = value.Any(x => char.IsLower(x));
            var isDigit = value.Any(x => char.IsDigit(x));

            if(!isLower && !isDigit)
            {
                throw new ArgumentException($"Value {value} not valid\nInvalid Password!");
            }

            this.userService.ChangePassword(userId, value);
        }
    }
}
