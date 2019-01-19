using RepositoryLayer.Abstraction;

namespace RepositoryLayer.RepositoryExtensions
{
    public interface IAccountRepository<TEntity, in TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
    }
}
