namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Models.Enums;
    using PhotoShare.Services.Contracts;

    public class ShareAlbumCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IAlbumService albumService;
        private readonly IAlbumRoleService albumRoleService;

        public ShareAlbumCommand(IUserService userService, IAlbumService albumService, IAlbumRoleService albumRoleService)
        {
            this.userService = userService;
            this.albumService = albumService;
            this.albumRoleService = albumRoleService;
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

            int albumId = int.Parse(data[0]);
            string username = data[1];
            string permission = data[2];

            bool albumExists = this.albumService.Exists(albumId);

            if(!albumExists)
            {
                throw new ArgumentException($"Album {albumId} not found!");
            }

            bool userExists = this.userService.Exists(username);

            if(!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
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
