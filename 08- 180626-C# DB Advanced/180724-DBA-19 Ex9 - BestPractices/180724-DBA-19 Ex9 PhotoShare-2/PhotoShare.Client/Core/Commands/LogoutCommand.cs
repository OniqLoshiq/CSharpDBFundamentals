using PhotoShare.Client.Core.Contracts;
using PhotoShare.Services.Contracts;
using System;

namespace PhotoShare.Client.Core.Commands
{
    public class LogoutCommand : ICommand
    {
        private readonly IUserSessionService userSessionService;

        public LogoutCommand(IUserSessionService userSessionService)
        {
            this.userSessionService = userSessionService;
        }

        public string Execute(string[] args)
        {
            if(!this.userSessionService.LoggedIn)
            {
                throw new InvalidOperationException("You should log in first in order to logout.");
            }

            string username = this.userSessionService.User.Username;

            this.userSessionService.Logout();

            return $"User {username} successfully logged out!";
        }
    }
}
