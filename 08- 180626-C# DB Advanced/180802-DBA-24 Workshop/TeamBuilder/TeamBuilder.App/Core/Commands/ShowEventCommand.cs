using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using TeamBuilder.App.Core.Commands.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;

namespace TeamBuilder.App.Core.Commands
{
    public class ShowEventCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(1, args);

            string eventName = args[0];

            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            var sb = new StringBuilder();

            using (var ctx = new TeamBuilderContext())
            {
                var @event = ctx.Events.Where(e => e.Name == eventName).OrderByDescending(e => e.StartDate).First();

                sb.AppendLine($"{@event.Name} {@event.StartDate.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture)} {@event.EndDate.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture)}")
                    .AppendLine(@event.Description)
                    .AppendLine("Teams:");

                foreach (var eventTeam in @event.ParticipatingEventTeams)
                {
                    sb.AppendLine($"- {eventTeam.Team.Name}");
                }
            }

            return sb.ToString().Trim();
        }
    }
}
