using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;

namespace TeamBuilder.App.Core.Commands
{
    public class DisbandCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(1, args);

            AuthenticationManager.Authorize();

            string teamName = args[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var currentUser = AuthenticationManager.GetGurrentUser();

            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            using (var ctx = new TeamBuilderContext())
            {
                var team = ctx.Teams.Single(t => t.Name == teamName);
                ctx.Teams.Remove(team);

                ctx.SaveChanges();
            }


            return $"{teamName} has disbanded!";
        }
    }
}
