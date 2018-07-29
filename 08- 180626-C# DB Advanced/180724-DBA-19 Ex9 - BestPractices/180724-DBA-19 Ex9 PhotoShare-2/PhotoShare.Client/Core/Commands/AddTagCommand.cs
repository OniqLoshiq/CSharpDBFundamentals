using PhotoShare.Client.Core.Contracts;
using PhotoShare.Client.Utilities;
using PhotoShare.Services.Contracts;
using System;

namespace PhotoShare.Client.Core.Commands
{
    public class AddTagCommand : ICommand
    {
        private readonly ITagService tagService;
        private readonly IUserSessionService userSessionService;

        public AddTagCommand(ITagService tagService, IUserSessionService userSessionService)
        {
            this.tagService = tagService;
            this.userSessionService = userSessionService;
        }

        public string Execute(string[] args)
        {
            string cmdName = this.GetType().Name;
            int cmdIndex = cmdName.IndexOf("Command");
            cmdName = cmdName.Substring(0, cmdIndex);

            if (args.Length != 1)
            {
                throw new InvalidOperationException($"Command {cmdName} not valid!");
            }

            if (!this.userSessionService.LoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            var tagName = args[0];

            var tagExists = this.tagService.Exists(tagName);

            if(tagExists)
            {
                throw new ArgumentException($"Tag {tagName} exists!");
            }

            tagName = tagName.ValidateOrTransform();

            var tag = this.tagService.AddTag(tagName);

            return $"Tag {tag.Name} was added successfully!";
        }
    }
}
