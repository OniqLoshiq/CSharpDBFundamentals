using AutoMapper.QueryableExtensions;
using PhotoShare.Data;
using PhotoShare.Models;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoShare.Services
{
    public class TownService : ITownService
    {
        private PhotoShareContext ctx;

        public TownService(PhotoShareContext ctx)
        {
            this.ctx = ctx;
        }

        public Town Add(string townName, string countryName)
        {
            var town = new Town
            {
                Name = townName,
                Country = countryName
            };

            this.ctx.Towns.Add(town);

            this.ctx.SaveChanges();

            return town;
        }

        public TModel ById<TModel>(int id) => this.By<TModel>(x => x.Id == id).SingleOrDefault();

        public TModel ByName<TModel>(string name) => this.By<TModel>(x => x.Name == name).SingleOrDefault();

        public bool Exists(int id) => this.ById<Town>(id) != null;

        public bool Exists(string name) => this.ByName<Town>(name) != null;

        private IEnumerable<TModel> By<TModel>(Func<Town, bool> predicate) => this.ctx.Towns.Where(predicate).AsQueryable().ProjectTo<TModel>();
    }
}
