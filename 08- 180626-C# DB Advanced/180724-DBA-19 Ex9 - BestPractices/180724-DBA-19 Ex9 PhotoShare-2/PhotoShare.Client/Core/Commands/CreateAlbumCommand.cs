namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Models.Enums;
    using Services.Contracts;


    public class CreateAlbumCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly ITagService tagService;
        private readonly IUserSessionService userSessionService;

        public CreateAlbumCommand(IAlbumService albumService, IUserService userService, ITagService tagService, IUserSessionService userSessionService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.tagService = tagService;
            this.userSessionService = userSessionService;
        }

        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public string Execute(string[] data)
        {
            string cmdName = this.GetType().Name;
            int cmdIndex = cmdName.IndexOf("Command");
            cmdName = cmdName.Substring(0, cmdIndex);

            if (data.Length < 3)
            {
                throw new InvalidOperationException($"Command {cmdName} not valid!");
            }

            if (!this.userSessionService.LoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string username = data[0];
            string albumTitle = data[1];
            string color = data[2];
            string[] tags = data.Skip(3).ToArray();

            if (this.userSessionService.User.Username != username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            var userExists = this.userService.Exists(username);

            if(!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            var albumExists = this.albumService.Exists(albumTitle);

            if(albumExists)
            {
                throw new ArgumentException($"Album {albumTitle} exists!");
            }

            var isValidColor = Enum.TryParse(color, out Color result);

            if(!isValidColor)
            {
                throw new ArgumentException($"Color {color} not found!");
            }

            foreach (var tag in tags)
            {
                var currentTag = this.tagService.Exists(tag.ValidateOrTransform());

                if(!currentTag)
                {
                    throw new ArgumentException("Invalid tags!");
                }
            }

            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = tags[i].ValidateOrTransform();

                var currentTag = this.tagService.Exists(tags[i]);

                if (!currentTag)
                {
                    throw new ArgumentException("Invalid tags!");
                }
            }

            var userId = this.userService.ByUsername<UserDto>(username).Id;

            this.albumService.Create(userId, albumTitle, color, tags);

            return $"Album {albumTitle} successfully created!";
        }
    }
}
