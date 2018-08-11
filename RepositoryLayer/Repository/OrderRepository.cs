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
    public class OrderRepository : IRepository<Order, Guid>, IOrderRepository<Order>
    {
        private IUnitOfWork _unitOfWork;
        private IDatabaseContextFactory _databaseContextFactory;

        public OrderRepository()
        {
            _databaseContextFactory = new DatabaseContextFactory();
        }

        public void Add(Order entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                _unitOfWork.context.Set<Order>().Add(entity);
                _unitOfWork.Commit();
            }
        }

        public void Delete(Order entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                Order existing = _unitOfWork.context.Set<Order>().Find(entity);
                if(existing != null)
                {
                    _unitOfWork.context.Set<Order>().Remove(entity);
                    _unitOfWork.Commit();
                }
            }
        }

        public Func<IQueryable<Order>, bool> Exists()
        {
            throw new NotImplementedException();
        }

        public Order Get(Guid key)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                return _unitOfWork.context.Set<Order>().Where(o => o.OrderId == key).FirstOrDefault();
            }
        }

        public void Update(Order entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                try
                {
                    var local = _unitOfWork.context.Set<Order>()
                        .Local
                        .FirstOrDefault(e => e.OrderId.Equals(entity.OrderId));
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
                    var clientValues = (Order)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry != null)
                    {
                        var databaseValues = (Order)databaseEntry.ToObject();

                        UpdateException updateException = new UpdateException(databaseValues);
                        throw updateException;
                    }
                }
            }
        }
    }
}
