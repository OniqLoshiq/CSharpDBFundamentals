namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Models.Enums;
    using PhotoShare.Services.Contracts;

    public class ShareAlbumCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IAlbumService albumService;
        private readonly IAlbumRoleService albumRoleService;
        private readonly IUserSessionService userSessionService;

        public ShareAlbumCommand(IUserService userService, IAlbumService albumService, IAlbumRoleService albumRoleService, IUserSessionService userSessionService)
        {
            this.userService = userService;
            this.albumService = albumService;
            this.albumRoleService = albumRoleService;
            this.userSessionService = userSessionService;
        }

        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public string Execute(string[] data)
        {
            string cmdName = this.GetType().Name;
            int cmdIndex = cmdName.IndexOf("Command");
            cmdName = cmdName.Substring(0, cmdIndex);

            if (data.Length != 3)
            {
                throw new InvalidOperationException($"Command {cmdName} not valid!");
            }

            if (!this.userSessionService.LoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            int albumId = int.Parse(data[0]);
            string username = data[1];
            string permission = data[2];

            bool albumExists = this.albumService.Exists(albumId);

            if(!albumExists)
            {
                throw new ArgumentException($"Album {albumId} not found!");
            }

            bool userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (!this.userSessionService.User.AlbumRoles.Any(x => x.Album.Id == albumId))
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            var albumRole = this.userSessionService.User.AlbumRoles.Where(a => a.AlbumId == albumId).Single().Role;

            if(albumRole != 0)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            bool isValidPermission = Enum.TryParse<Role>(permission, out _);

            if(!isValidPermission)
            {
                throw new ArgumentException("Permission must be either \"Owner\" or \"Viewer\"!");
            }

            int userId = this.userService.ByUsername<UserDto>(username).Id;
            string album = this.albumService.ById<AlbumDto>(albumId).Name;

            this.albumRoleService.PublishAlbumRole(albumId, userId, permission);

            return $"Username {username} added to album {album} ({permission})";
        }
    }
}
