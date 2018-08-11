using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Abstraction
{
    public interface IBulkRepository<TEntity, in TKey> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

    }
}
