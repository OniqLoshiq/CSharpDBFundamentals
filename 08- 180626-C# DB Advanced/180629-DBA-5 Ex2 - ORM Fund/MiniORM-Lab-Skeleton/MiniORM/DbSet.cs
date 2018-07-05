﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MiniORM
{
    public class DbSet<TEntity> : ICollection<TEntity>
        where TEntity : class, new()
    {
        internal DbSet(IEnumerable<TEntity> entities)
        {
            this.Entities = entities.ToList();

            this.ChangeTracker = new ChangeTracker<TEntity>(entities);
        }

        internal IList<TEntity> Entities { get; }

        internal ChangeTracker<TEntity> ChangeTracker { get; set; }

        public int Count => this.Entities.Count;

        public bool IsReadOnly => this.Entities.IsReadOnly;

        public void Add(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null!");
            }

            this.Entities.Add(item);

            this.ChangeTracker.Add(item);
        }

        public void Clear()
        {
            foreach (var entity in this.Entities)
            {
                this.ChangeTracker.Remove(entity);
            }

            this.Entities.Clear();
        }

        public bool Contains(TEntity item) => this.Entities.Contains(item);

        public void CopyTo(TEntity[] array, int arrayIndex) => this.Entities.CopyTo(array, arrayIndex);

        public IEnumerator<TEntity> GetEnumerator() => this.Entities.GetEnumerator();

        public bool Remove(TEntity item)
        {
            var removed = this.Entities.Remove(item);
            if (removed)
            {
                this.ChangeTracker.Remove(item);
            }

            return removed;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();


    }
}