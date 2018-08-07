using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class LoginCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(2, args);

            if(AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            string username = args[0];
            string password = args[1];

            User user = this.GetUserByCredentials(username, password);

            if(user == null || user.IsDeleted)
            {
                throw new ArgumentException(Constants.ErrorMessages.UserOrPasswordIsInvalid);
            }

            AuthenticationManager.Login(user);

            return $"User {username} successfully logged in!";
        }

        private User GetUserByCredentials(string username, string password)
        {
            using (var ctx = new TeamBuilderContext())
            {
                return ctx.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            }
        }
    }
}
