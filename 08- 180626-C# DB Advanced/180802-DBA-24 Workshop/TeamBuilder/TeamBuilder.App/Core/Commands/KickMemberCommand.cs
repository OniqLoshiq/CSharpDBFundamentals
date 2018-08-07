using System;
using System.Collections.Generic;
using System.Linq;
using TeamBuilder.App.Core.Commands.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;

namespace TeamBuilder.App.Core.Commands
{
    public class KickMemberCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(2, args);

            AuthenticationManager.Authorize();

            string teamName = args[0];
            string username = args[1];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (!CommandHelper.IsUserExisting(username))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserNotFound, username));
            }

            if (!CommandHelper.IsMemberOfTeam(teamName, username))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.NotMemberOfTeam, username, teamName));
            }

            var currentUser = AuthenticationManager.GetGurrentUser();

            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (currentUser.Username == username)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.CantKickCreatorOfTeam);
            }

            using (var ctx = new TeamBuilderContext())
            {
                var userTeam = ctx.UserTeams.Single(ut => ut.User.Username == username && ut.Team.Name == teamName);

                ctx.UserTeams.Remove(userTeam);
                ctx.SaveChanges();
            }


            return $"User {username} was kicked from {teamName}!";
        }
    }
}
