using RepositoryLayer.Abstraction;
using System;
using System.Collections.Generic;

namespace RepositoryLayer.RepositoryExtensions
{
    public interface IEventRepository<TEntity, in TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        IEnumerable<TEntity> GetOrdersEvents(Guid id);
    }
}
