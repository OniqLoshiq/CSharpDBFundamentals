namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Models;
    using PhotoShare.Services.Contracts;

    public class AddFriendCommand : ICommand
    {
        private readonly IUserService userService;

        public AddFriendCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // AddFriend <username1> <username2>
        public string Execute(string[] data)
        {
            string cmdName = this.GetType().Name;
            int cmdIndex = cmdName.IndexOf("Command");
            cmdName = cmdName.Substring(0, cmdIndex);

            if (data.Length < 2 || data.Length > 2)
            {
                throw new InvalidOperationException($"Command {cmdName} not valid!");
            }

            string username = data[0];
            string friendUsername = data[1];

            CheckUser(username);
            CheckUser(friendUsername);

            var user = this.userService.ByUsername<UserFriendsDto>(username);
            var friend = this.userService.ByUsername<UserFriendsDto>(friendUsername);

            bool isSendRequestFromUser = user.Friends.Any(x => x.Username == friend.Username);
            bool isSendRequestFromFriend = friend.Friends.Any(x => x.Username == user.Username);

            if (isSendRequestFromUser && isSendRequestFromFriend)
            {
                throw new InvalidOperationException($"{friendUsername} is already a friend to {username}");
            }
            else if(isSendRequestFromUser && !isSendRequestFromFriend)
            {
                throw new InvalidOperationException("Request is already send!");
            }

            this.userService.AddFriend(user.Id, friend.Id);

            return $"Friend {username} added to {friendUsername}!";
        }

        private void CheckUser(string username)
        {
            var userExists = this.userService.Exists(username);

            if(!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }
        }
    }
}
