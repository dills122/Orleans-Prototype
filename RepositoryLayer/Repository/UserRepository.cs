using DataModels.Exceptions;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.ContextFactory;
using RepositoryLayer.RepositoryExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Repository
{
    public class UserRepository : IRepository<User, string>, IUserRepository<User>
    {
        private IUnitOfWork _unitOfWork;
        private IDatabaseContextFactory _databaseContextFactory;
        public UserRepository()
        {
            _databaseContextFactory = new DatabaseContextFactory();
        }
        public void Add(User entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                _unitOfWork.context.Set<User>().Add(entity);
                _unitOfWork.Commit();
            }
        }

        public void Delete(User entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                User existing = _unitOfWork.context.Set<User>().Find(entity);
                if (existing != null)
                {
                    _unitOfWork.context.Set<User>().Remove(existing);
                    _unitOfWork.Commit();
                }
            }
        }

        public Func<IQueryable<User>, bool> Exists()
        {
            throw new NotImplementedException();
        }

        public User Get(string key)
        {
            using(_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                return _unitOfWork.context.Set<User>().Where(c => c.Username == key).FirstOrDefault();
            }
        }

        public void Update(User entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                try
                {
                    var local = _unitOfWork.context.Set<User>()
                        .Local
                        .FirstOrDefault(e => e.Username.Equals(entity.Username));
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
                    var clientValues = (User)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry != null)
                    {
                        var databaseValues = (User)databaseEntry.ToObject();

                        UpdateException updateException = new UpdateException(databaseValues);
                        throw updateException;
                    }
                }
            }
        }
    }
}
