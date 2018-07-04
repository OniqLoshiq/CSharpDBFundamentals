using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MiniORM
{
    internal class ChangeTracker<TEntity>
        where TEntity : class, new()
    {
        private readonly List<TEntity> allEntities;

        private readonly List<TEntity> added;

        private readonly List<TEntity> removed;

        public ChangeTracker(IEnumerable<TEntity> entities)
        {
            this.added = new List<TEntity>();
            this.removed = new List<TEntity>();

            this.allEntities = CloneEntities(entities).ToList();
        }

        private static IEnumerable<TEntity> CloneEntities(IEnumerable<TEntity> entities)
        {
            var clonedEntities = new List<TEntity>();

            var propertiesToClone = typeof(TEntity).GetProperties()
                                    .Where(pi => AllowedSqlTypes.SqlTypes.Contains(pi.PropertyType))
                                    .ToArray();

            foreach (var entity in entities)
            {
                var clonedInstance = new TEntity();

                foreach (var property in propertiesToClone)
                {
                    var originalValue = property.GetValue(entity);

                    property.SetValue(clonedInstance, originalValue);
                }

                clonedEntities.Add(clonedInstance);
            }

            return clonedEntities;
        }

        public IReadOnlyCollection<TEntity> AllEntities => this.allEntities.AsReadOnly();

        public IReadOnlyCollection<TEntity> Added => this.added.AsReadOnly();

        public IReadOnlyCollection<TEntity> Removed => this.removed.AsReadOnly();

        public void Add(TEntity item) => this.added.Add(item);

        public void Remove(TEntity item) => this.removed.Add(item);

        private static IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKeys, TEntity entity)
        {
            var primaryKeyProperties = primaryKeys
                                       .Select(pk => pk.GetValue(entity))
                                       .ToArray();

            return primaryKeyProperties;
        }

        public IEnumerable<TEntity> GetModifiedEntities(DbSet<TEntity> dbSet)
        {
            var modifiedEntities = new List<TEntity>();

            var primaryKeys = typeof(TEntity).GetProperties()
                              .Where(pi => pi.HasAttribute<KeyAttribute>())
                              .ToArray();

            foreach (var clonedEntity in this.AllEntities)
            {
                var primaryKeyValues = GetPrimaryKeyValues(primaryKeys, clonedEntity).ToArray();

                var entity = dbSet.Entities.Single(e => GetPrimaryKeyValues(primaryKeys, e).SequenceEqual(primaryKeyValues));

                var isModified = IsModified(entity, clonedEntity);

                if (isModified)
                {
                    modifiedEntities.Add(entity);
                }
            }
            return modifiedEntities;
        }

        private static bool IsModified(TEntity entity, TEntity clonedEntity)
        {
            var monitoredProperties = typeof(TEntity).GetProperties()
                                      .Where(pi => AllowedSqlTypes.SqlTypes.Contains(pi.PropertyType))
                                      .ToArray();

            var modifiedProperties = monitoredProperties
                                     .Where(pi => !Equals(pi.GetValue(entity), pi.GetValue(clonedEntity)))
                                     .ToArray();

            bool isModified = modifiedProperties.Any();

            return isModified;
        }
    }
}