
namespace RepositoryLayer.Abstraction
{
    public interface IGenericRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
    }
}
