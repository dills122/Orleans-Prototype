using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.RepositoryExtensions
{
   public interface IUserRepository<TEntity>
    {
        Func<IQueryable<TEntity>, bool> Exists();
    }
}
