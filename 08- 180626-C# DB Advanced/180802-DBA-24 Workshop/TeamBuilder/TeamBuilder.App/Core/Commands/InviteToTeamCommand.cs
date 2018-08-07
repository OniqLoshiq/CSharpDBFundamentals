using System;
using System.Collections.Generic;
using System.Linq;
using TeamBuilder.App.Core.Commands.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class InviteToTeamCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(2, args);

            AuthenticationManager.Authorize();

            string teamName = args[0];
            string username = args[1];

            if(!CommandHelper.IsTeamExisting(teamName) || !CommandHelper.IsUserExisting(username))
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);
            }

            var currentUser = AuthenticationManager.GetGurrentUser();

            using (var ctx = new TeamBuilderContext())
            {
                var user = ctx.Users.Single(u => u.Username == username);

                if((!CommandHelper.IsUserCreatorOfTeam(teamName,currentUser) && !CommandHelper.IsMemberOfTeam(teamName, currentUser.Username))
                    || CommandHelper.IsMemberOfTeam(teamName, username))
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
                }

                if (CommandHelper.IsInviteExisting(teamName, user))
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.InviteIsAlreadySent);
                }

                var team = ctx.Teams.Single(t => t.Name == teamName);

                var invitation = new Invitation()
                {
                    InvitedUserId = user.Id,
                    TeamId = team.Id,
                };

                ctx.Invitations.Add(invitation);
                ctx.SaveChanges();
            }

            return $"Team {teamName} invited {username}!";
        }
    }
}
