namespace RepositoryLayer.Abstraction
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        TEntity Get(TKey key);
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
