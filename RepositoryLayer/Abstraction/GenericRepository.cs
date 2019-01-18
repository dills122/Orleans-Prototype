using DataModels.Exceptions;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.ContextFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Abstraction
{
    public abstract class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
    {

        protected IUnitOfWork _unitOfWork;
        protected readonly IDatabaseContextFactory _databaseContextFactory;

        protected GenericRepository()
        {
            _databaseContextFactory = new DatabaseContextFactory();
        }

        public void Add(TEntity entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                _unitOfWork.context.Set<TEntity>().Add(entity);
                _unitOfWork.Commit();
            }
        }

        public void Delete(TEntity entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                TEntity existing = _unitOfWork.context.Set<TEntity>().Find(entity);
                if (existing != null)
                {
                    _unitOfWork.context.Set<TEntity>().Remove(entity);
                    _unitOfWork.Commit();
                }
            }
        }

        protected abstract bool MatchKey(TEntity entity, TKey key);
        protected abstract bool MatchEntity(TEntity source, TEntity target);
        protected abstract bool MatchIntForGetList(TEntity source, int foreignKey);

        public TEntity Get(TKey key)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                return _unitOfWork.context.Set<TEntity>().Where(o => MatchKey(o, key)).FirstOrDefault();
            }
        }

        public IEnumerable<TEntity> GetListByIntKey(int foreignKey)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                return _unitOfWork.context.Set<TEntity>().Where(o => MatchIntForGetList(o, foreignKey));
            }
        }

        public void Update(TEntity entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                try
                {
                    var local = _unitOfWork.context.Set<TEntity>()
                        .Local
                        .FirstOrDefault(e => MatchEntity(e, entity));
                    if (local != null)
                    {
                        _unitOfWork.context.Entry(local).State = EntityState.Detached;
                    }
                    _unitOfWork.context.Entry(entity).State = EntityState.Modified;

                    _unitOfWork.Commit();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry != null)
                    {
                        var databaseValues = (TEntity)databaseEntry.ToObject();

                        UpdateException updateException = new UpdateException(databaseValues);
                        throw updateException;
                    }
                }
            }
        }
    }
}
