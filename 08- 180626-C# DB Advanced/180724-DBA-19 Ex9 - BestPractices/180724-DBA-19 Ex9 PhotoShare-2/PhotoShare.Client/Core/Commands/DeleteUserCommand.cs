namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Dtos;
    using Contracts;
    using Services.Contracts;

    public class DeleteUserCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IUserSessionService userSessionService;

        public DeleteUserCommand(IUserService userService, IUserSessionService userSessionService)
        {
            this.userService = userService;
            this.userSessionService = userSessionService;
        }

        // DeleteUser <username>
        public string Execute(string[] data)
        {
            string cmdName = this.GetType().Name;
            int cmdIndex = cmdName.IndexOf("Command");
            cmdName = cmdName.Substring(0, cmdIndex);

            if (data.Length != 1)
            {
                throw new InvalidOperationException($"Command {cmdName} not valid!");
            }

            if (!this.userSessionService.LoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string username = data[0];

            if (this.userSessionService.User.Username != username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            this.userService.Delete(username);


            return $"User {username} was deleted from the database!";
        }
    }
}
