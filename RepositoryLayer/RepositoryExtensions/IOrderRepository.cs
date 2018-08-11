using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.RepositoryExtensions
{
    public interface IOrderRepository<TEntity> 
    {
        Func<IQueryable<TEntity>, bool> Exists();
    }
}
