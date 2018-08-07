using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class AddTeamToCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(2, args);

            AuthenticationManager.Authorize();

            string teamName = args[1];
            string eventName = args[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if(!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            var currentUser = AuthenticationManager.GetGurrentUser();

            if(!CommandHelper.IsUserCreatorOfEvent(eventName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            using (var ctx = new TeamBuilderContext())
            {
                var @event = ctx.Events.Where(e => e.Name == eventName).OrderByDescending(e => e.StartDate).First();

                if (ctx.EventTeams.Any(et => et.Event.Id == @event.Id && et.Team.Name == teamName))
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.CannotAddSameTeamTwice);
                }

                var team = ctx.Teams.Single(t => t.Name == teamName);

                var eventTeam = new EventTeam()
                {
                    EventId = @event.Id,
                    TeamId = team.Id
                };

                ctx.EventTeams.Add(eventTeam);
                ctx.SaveChanges();
            }
                

            return $"Team {teamName} added for {eventName}!";
        }
    }
}
