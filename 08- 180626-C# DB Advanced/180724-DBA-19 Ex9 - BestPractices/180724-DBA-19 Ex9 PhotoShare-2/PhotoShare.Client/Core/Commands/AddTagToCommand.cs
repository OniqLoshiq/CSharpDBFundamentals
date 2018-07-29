namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Services.Contracts;

    public class AddTagToCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly IAlbumTagService albumTagService;
        private readonly ITagService tagService;
        private readonly IUserSessionService userSessionService;

        public AddTagToCommand(IAlbumService albumService, IAlbumTagService albumTagService, ITagService tagService, IUserSessionService userSessionService)
        {
            this.albumService = albumService;
            this.albumTagService = albumTagService;
            this.tagService = tagService;
            this.userSessionService = userSessionService;
        }

        // AddTagTo <albumName> <tag>
        public string Execute(string[] args)
        {
            string cmdName = this.GetType().Name;
            int cmdIndex = cmdName.IndexOf("Command");
            cmdName = cmdName.Substring(0, cmdIndex);

            if (args.Length != 2)
            {
                throw new InvalidOperationException($"Command {cmdName} not valid!");
            }

            if (!this.userSessionService.LoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string albumName = args[0];
            string tagName = args[1];


            var tag = tagName.ValidateOrTransform();

            if (!this.albumService.Exists(albumName) || !this.tagService.Exists(tag))
            {
                throw new ArgumentException("Either tag or album do not exist!");
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

            int albumId = this.albumService.ByName<AlbumDto>(albumName).Id;
            int tagId = this.tagService.ByName<TagDto>(tag).Id;

            this.albumTagService.AddTagTo(albumId, tagId);
            
            return $"Tag {tagName} added to {albumName}!";
        }
    }
}
