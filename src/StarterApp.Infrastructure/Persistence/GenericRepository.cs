using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Common.Interfaces;
using StarterApp.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StarterApp.Infrastructure.Persistence
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
        }
        public IUnitOfWork UnitOfWork => DbContext;
        public IQueryable<TEntity> Query => DbSet.AsNoTracking();
        public IQueryable<TEntity> Entity => DbSet;

        public async Task<TEntity> GetById(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            //entity.Delete();
            DbSet.Remove(entity);
        }

        public void UpdateChildCollection<TCollection>(TEntity entity, Expression<Func<TEntity, IEnumerable<TCollection>>> exp,
            IEnumerable<TCollection> newItems) where TCollection : BaseEntity
        {
            var dbEntity = entity;
            var dbItemsEntry = DbContext.Entry(entity).Collection(exp);
            var accessor = dbItemsEntry.Metadata.GetCollectionAccessor();
            var dbItemsMap = (dbItemsEntry.CurrentValue)
                .ToDictionary(x => x.Id);

            foreach (var item in newItems)
            {
                if (!dbItemsMap.TryGetValue(item.Id, out var oldItem))
                    accessor.Add(dbEntity, item, false);
                else
                {
                    DbContext.Entry(oldItem).CurrentValues.SetValues(item);
                    dbItemsMap.Remove(item.Id);
                }
            }

            foreach (var oldItem in dbItemsMap.Values)
                accessor.Remove(dbEntity, oldItem);
        }
    }
}
