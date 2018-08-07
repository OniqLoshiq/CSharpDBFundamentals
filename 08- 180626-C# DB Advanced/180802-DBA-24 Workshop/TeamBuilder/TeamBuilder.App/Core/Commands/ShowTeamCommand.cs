using System;
using System.Linq;
using System.Text;
using TeamBuilder.App.Core.Commands.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;

namespace TeamBuilder.App.Core.Commands
{
    public class ShowTeamCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(1, args);

            string teamName = args[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var sb = new StringBuilder();

            using (var ctx = new TeamBuilderContext())
            {
                var team = ctx.Teams.Single(t => t.Name == teamName);

                sb.AppendLine($"{team.Name} {team.Acronym}")
                    .AppendLine("Members:");

                foreach (var userTeam in team.Members)
                {
                    sb.AppendLine($"- {userTeam.User.Username}");
                }
            }

            return sb.ToString().Trim();
        }
    }
}
