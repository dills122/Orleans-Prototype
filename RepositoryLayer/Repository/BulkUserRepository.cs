using DataModels.Models;
using RepositoryLayer.Abstraction;
using RepositoryLayer.ContextFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Repository
{
    public class BulkUserRepository : IBulkRepository<User, string>
    {
        private IUnitOfWork _unitOfWork;
        private IDatabaseContextFactory _databaseContextFactory;
        public BulkUserRepository()
        {
            _databaseContextFactory = new DatabaseContextFactory();
            //_unitOfWork = new UnitOfWork(new OrleansContext());
        }

        public IEnumerable<User> GetAll()
        {
            using(_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                return _unitOfWork.context.Set<User>().ToList();
            }
        }
    }
}
