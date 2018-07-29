using AutoMapper.QueryableExtensions;
using PhotoShare.Data;
using PhotoShare.Models;
using PhotoShare.Models.Enums;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoShare.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly PhotoShareContext ctx;

        public AlbumService(PhotoShareContext ctx)
        {
            this.ctx = ctx;
        }

        public TModel ById<TModel>(int id) => this.By<TModel>(x => x.Id == id).SingleOrDefault();

        public TModel ByName<TModel>(string name) => this.By<TModel>(x => x.Name == name).SingleOrDefault();

        public Album Create(int userId, string albumTitle, string bgColor, string[] tags)
        {
            var backgroundColor = Enum.Parse<Color>(bgColor, true);

            var album = new Album
            {
                Name = albumTitle,
                BackgroundColor = backgroundColor
            };

            this.ctx.Albums.Add(album);

            this.ctx.SaveChanges();

            var albumRole = new AlbumRole
            {
                UserId = userId,
                Album = album
            };

            this.ctx.AlbumRoles.Add(albumRole);
            this.ctx.SaveChanges();

            foreach (var tag in tags)
            {
                var currentTagId = this.ctx.Tags.FirstOrDefault(x => x.Name == tag).Id;

                var albumTag = new AlbumTag
                {
                    Album = album,
                    TagId = currentTagId
                };

                this.ctx.AlbumTags.Add(albumTag);
            }

            this.ctx.SaveChanges();

            return album;
        }

        public bool Exists(int id) => this.ById<Album>(id) != null;

        public bool Exists(string name) => this.ByName<Album>(name) != null;

        private IEnumerable<TModel> By<TModel>(Func<Album, bool> predicate) => this.ctx.Albums.Where(predicate).AsQueryable().ProjectTo<TModel>();
    }
}
