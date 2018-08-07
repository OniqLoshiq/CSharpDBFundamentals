using System;
using TeamBuilder.App.Core.Commands.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class DeleteUserCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(0, args);

            User currentUser = AuthenticationManager.GetGurrentUser();

            using (var ctx = new TeamBuilderContext())
            {
                ctx.Users.Attach(currentUser);

                currentUser.IsDeleted = true;
                ctx.SaveChanges();

                AuthenticationManager.Logout();
            }

            return $"User {currentUser.Username} was deleted successfully!";
        }
    }
}
