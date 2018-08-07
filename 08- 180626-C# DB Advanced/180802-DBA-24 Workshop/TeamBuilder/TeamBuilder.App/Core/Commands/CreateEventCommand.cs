using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using TeamBuilder.App.Core.Commands.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class CreateEventCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(6, args);

            AuthenticationManager.Authorize();

            string eventName = args[0];
            string description = args[1];
            string startDateStr = args[2] + " " + args[3];
            string endDateStr = args[4] + " " + args[5];
            
            DateTime startDate = ValidateDateParsing(startDateStr);
            DateTime endDate = ValidateDateParsing(endDateStr);

            if (startDate >= endDate)
            {
                throw new ArgumentException(Constants.ErrorMessages.StartDateIsAfterEndDate);
            }

            var currentUser = AuthenticationManager.GetGurrentUser();

            var newEvent = new Event()
            {
                Name = eventName,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                CreatorId = currentUser.Id
            };

            if(!IsValid(newEvent))
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidEventArgs);
            }

            using (var ctx = new TeamBuilderContext())
            {
                ctx.Events.Add(newEvent);
                ctx.SaveChanges();
            }

            return $"Event {eventName} was created successfully!";
        }

        private DateTime ValidateDateParsing(string dateStr)
        {
            DateTime date;

            if (!DateTime.TryParseExact(dateStr, Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat);
            }

            return date;
        }

        private bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true); 
        }
    }
}
