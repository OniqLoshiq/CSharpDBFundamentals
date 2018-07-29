namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Services.Contracts;

    public class AcceptFriendCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IUserSessionService userSessionService;

        public AcceptFriendCommand(IUserService userService, IUserSessionService userSessionService)
        {
            this.userService = userService;
            this.userSessionService = userSessionService;
        }

        // AcceptFriend <username1> <username2>
        public string Execute(string[] data)
        {
            string cmdName = this.GetType().Name;
            int cmdIndex = cmdName.IndexOf("Command");
            cmdName = cmdName.Substring(0, cmdIndex);

            if(data.Length < 2 || data.Length > 2)
            {
                throw new InvalidOperationException($"Command {cmdName} not valid!");
            }

            if (!this.userSessionService.LoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string username1 = data[0];
            string username2 = data[1];

            if (this.userSessionService.User.Username != username1)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            CheckUser(username1);
            CheckUser(username2);

            var user = this.userService.ByUsername<UserFriendsDto>(username1);
            var friend = this.userService.ByUsername<UserFriendsDto>(username2);

            bool isSendRequestFromUser = user.Friends.Any(x => x.Username == friend.Username);
            bool isSendRequestFromFriend = friend.Friends.Any(x => x.Username == user.Username);

            if(!isSendRequestFromFriend)
            {
                throw new InvalidOperationException($"{username2} has not added {username1} as a friend");
            }

            if(isSendRequestFromUser)
            {
                throw new InvalidOperationException($"{username2} is already a friend to {username1}");
            }

            int userId = this.userService.ByUsername<UserFriendsDto>(username1).Id;
            int friendId = this.userService.ByUsername<UserFriendsDto>(username2).Id;

            this.userService.AcceptFriend(userId, friendId);

            return $"{username1} accepted {username2} as a friend";
        }

        private void CheckUser(string username)
        {
            var userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException($"{username} not found!");
            }
        }
    }
}
