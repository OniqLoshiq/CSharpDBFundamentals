using System;
using System.Collections.Generic;
using System.Linq;
using TeamBuilder.App.Core.Commands.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;

namespace TeamBuilder.App.Core.Commands
{
    public class DeclineInviteCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(1, args);

            AuthenticationManager.Authorize();

            string teamName = args[0];

            if(!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var currentUser = AuthenticationManager.GetGurrentUser();

            if (!CommandHelper.IsInviteExisting(teamName, currentUser))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InviteNotFound, teamName));
            }

            using (var ctx = new TeamBuilderContext())
            {
                var invitation = ctx.Invitations.Single(i => i.Team.Name == teamName && i.InvitedUserId == currentUser.Id);
                invitation.IsActive = false;

                ctx.SaveChanges();
            }

            return $"Invite from {teamName} declined!";
        }
    }
}
