using PhotoShare.Client.Core.Contracts;
using PhotoShare.Client.Core.Dtos;
using PhotoShare.Services.Contracts;
using System;
using System.Linq;
using System.Text;

namespace PhotoShare.Client.Core.Commands
{
    public class PrintFriendsListCommand : ICommand
    {
        private readonly IUserService userService;

        public PrintFriendsListCommand(IUserService userService)
        {
            this.userService = userService;
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

            string username = args[0];

            bool userExists = this.userService.Exists(username);

            if(!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            var friends = this.userService.ByUsername<UserFriendsDto>(username).Friends.ToList();

            if(friends.Count == 0)
            {
                return "No friends for this user. :(";
            }

            var sb = new StringBuilder();

            sb.AppendLine("Friends:");

            foreach (var friend in friends)
            {
                sb.AppendLine($"-{friend.Username}");
            }

            return sb.ToString().Trim();
        }
    }
}
