using PhotoShare.Data;
using PhotoShare.Models;
using PhotoShare.Services.Contracts;

namespace PhotoShare.Services
{
    public class AlbumTagService : IAlbumTagService
    {
        private PhotoShareContext ctx;

        public AlbumTagService(PhotoShareContext ctx)
        {
            this.ctx = ctx;
        }

        public AlbumTag AddTagTo(int albumId, int tagId)
        {
            var albumTag = new AlbumTag
            {
                AlbumId = albumId,
                TagId = tagId
            };

            this.ctx.AlbumTags.Add(albumTag);

            this.ctx.SaveChanges();

            return albumTag;
        }
    }
}
