using RepositoryLayer.Abstraction;

namespace RepositoryLayer.RepositoryExtensions
{
    public interface ITransactionRepository<TEntity, in TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
    }
}
