namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Services.Contracts;

    public class AddTagToCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly IAlbumTagService albumTagService;
        private readonly ITagService tagService;

        public AddTagToCommand(IAlbumService albumService, IAlbumTagService albumTagService, ITagService tagService)
        {
            this.albumService = albumService;
            this.albumTagService = albumTagService;
            this.tagService = tagService;
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

            string albumName = args[0];
            string tagName = args[1];
            
            var tag = tagName.ValidateOrTransform();

            if (!this.albumService.Exists(albumName) || !this.tagService.Exists(tag))
            {
                throw new ArgumentException("Either tag or album do not exist!");
            }

            int albumId = this.albumService.ByName<AlbumDto>(albumName).Id;
            int tagId = this.tagService.ByName<TagDto>(tag).Id;

            this.albumTagService.AddTagTo(albumId, tagId);
            
            return $"Tag {tagName} added to {albumName}!";
        }
    }
}
