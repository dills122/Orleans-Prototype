using RepositoryLayer.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.RepositoryExtensions
{
    public interface IEventRepository<TEntity, in TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        IEnumerable<TEntity> GetOrdersEvents(Guid id);
    }
}
