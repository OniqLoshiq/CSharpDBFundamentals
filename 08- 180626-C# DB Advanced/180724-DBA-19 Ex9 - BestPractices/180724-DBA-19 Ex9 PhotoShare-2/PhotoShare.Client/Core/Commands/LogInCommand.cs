using PhotoShare.Client.Core.Contracts;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoShare.Client.Core.Commands
{
    public class LoginCommand : ICommand
    {
        private readonly IUserSessionService userSessionService;

        public LoginCommand(IUserSessionService userSessionService)
        {
            this.userSessionService = userSessionService;
        }

        public string Execute(string[] args)
        {
            var username = args[0];
            var password = args[1];

            if(this.userSessionService.LoggedIn)
            {
                throw new ArgumentException("You should logout first!");
            }

            this.userSessionService.Login(username, password);

            if(this.userSessionService.User == null)
            {
                throw new ArgumentException("Invalid username or password!");
            }

            return $"User {username} successfully logged in!";
        }
    }
}
