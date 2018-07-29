namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Dtos;
    using Contracts;
    using Services.Contracts;
    using System.Linq;

    public class UploadPictureCommand : ICommand
    {
        private readonly IPictureService pictureService;
        private readonly IAlbumService albumService;
        private readonly IUserSessionService userSessionService;

        public UploadPictureCommand(IPictureService pictureService, IAlbumService albumService, IUserSessionService userSessionService)
        {
            this.pictureService = pictureService;
            this.albumService = albumService;
            this.userSessionService = userSessionService;
        }

        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
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

            string albumName = data[0];
            string pictureTitle = data[1];
            string path = data[2];

            var albumExists = this.albumService.Exists(albumName);

            if (!albumExists)
            {
                throw new ArgumentException($"Album {albumName} not found!");
            }

            if (!this.userSessionService.User.AlbumRoles.Any(x => x.Album.Name == albumName))
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            var albumRole = this.userSessionService.User.AlbumRoles.Where(a => a.Album.Name == albumName).Single().Role;

            if (albumRole != 0)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            var albumId = this.albumService.ByName<AlbumDto>(albumName).Id;

            var picture = this.pictureService.Create(albumId, pictureTitle, path);

            return $"Picture {pictureTitle} added to {albumName}!";
        }
    }
}
