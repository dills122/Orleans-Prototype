using RepositoryLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.RepositoryExtensions
{
    public interface IOrderRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        Func<IQueryable<TEntity>, bool> Exists();
        IEnumerable<TKey> GetOrderIds(string key);
        //Hacks since all ids non string use Guids
        IEnumerable<TKey> GetAssociatedEvents(TKey key);
        IEnumerable<TKey> GetAssociatedDocuments(TKey key);
    }
}
