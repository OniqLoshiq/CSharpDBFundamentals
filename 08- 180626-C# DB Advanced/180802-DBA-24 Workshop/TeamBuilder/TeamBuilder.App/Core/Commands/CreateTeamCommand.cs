using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TeamBuilder.App.Core.Commands.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class CreateTeamCommand : ICommand
    {
        public string Execute(string[] args)
        {
            if(args.Length != 2)
            {
                Check.CheckLength(3, args);
            }

            AuthenticationManager.Authorize();

            string teamName = args[0];
            string acronym = args[1];
            string description = args.Length == 3 ? args[3] : null;

            if(CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamExists, teamName));
            }

            if(acronym.Length != 3)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InvalidAcronym, acronym));
            }

            var currentUser = AuthenticationManager.GetGurrentUser();

            var team = new Team()
            {
                Name = teamName,
                Acronym = acronym,
                Description = description,
                CreatorId = currentUser.Id
            };

            if(!IsValid(team))
            {
                throw new ArgumentException("Invalid team arguments!");
            }

            using (var ctx = new TeamBuilderContext())
            {
                ctx.Teams.Add(team);
                ctx.SaveChanges();
            }

            return $"Team {teamName} successfully created!";
        }

        private bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
