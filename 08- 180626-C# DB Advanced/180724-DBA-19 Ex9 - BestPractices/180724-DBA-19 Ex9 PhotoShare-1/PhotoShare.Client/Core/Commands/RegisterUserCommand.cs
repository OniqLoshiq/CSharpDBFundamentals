namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Services.Contracts;

    public class RegisterUserCommand : ICommand
    {
        private readonly IUserService userService;

        public RegisterUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // RegisterUser <username> <password> <repeat-password> <email>
        public string Execute(string[] data)
        {
            string cmdName = this.GetType().Name;
            int cmdIndex = cmdName.IndexOf("Command");
            cmdName = cmdName.Substring(0, cmdIndex);

            if (data.Length != 4)
            {
                throw new InvalidOperationException($"Command {cmdName} not valid!");
            }

            string username = data[0];
            string password = data[1];
            string repeatPassword = data[2];
            string email = data[3];

            var userExists = this.userService.Exists(username);

            if (userExists)
            {
                throw new InvalidOperationException($"Username {username} is already taken!");
            }

            if (password != repeatPassword)
            {
                throw new ArgumentException("Passwords do not match");
            }

            var registerRegisterDto = new RegisterUserDto
            {
                Username = username,
                Password = password,
                Email = email
            };

            if(!IsValid(registerRegisterDto))
            {
                throw new ArgumentException("Invalid data");
            }

            this.userService.Register(username, password, email);

            return $"User {username} was registered successfully!";
        }

        private bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
