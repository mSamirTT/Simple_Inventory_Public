using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StarterApp.Core.Common.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IUnitOfWork UnitOfWork { get;  }
        Task<TEntity> GetById(long id);
        IQueryable<TEntity> Query { get; }
        IQueryable<TEntity> Entity { get; }
        void Insert(TEntity entity);
        void Delete(TEntity entity);
        void UpdateChildCollection<TCollection>(TEntity entity, Expression<Func<TEntity, IEnumerable<TCollection>>> exp,
            IEnumerable<TCollection> newItems) where TCollection : BaseEntity;
    }
}
