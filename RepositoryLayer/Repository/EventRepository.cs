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
    public class EventRepository : IEventRepository<Event, Guid>
    {
        private IUnitOfWork _unitOfWork;
        private IDatabaseContextFactory _databaseContextFactory;

        public EventRepository()
        {
            _databaseContextFactory = new DatabaseContextFactory();
        }

        public void Add(Event entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                _unitOfWork.context.Set<Event>().Add(entity);
                _unitOfWork.Commit();
            }
        }

        public void Delete(Event entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                Event existing = _unitOfWork.context.Set<Event>().Find(entity);
                if (existing != null)
                {
                    _unitOfWork.context.Set<Event>().Remove(entity);
                    _unitOfWork.Commit();
                }
            }
        }

        public Event Get(Guid key)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                return _unitOfWork.context.Set<Event>().Where(e => e.EventId == key).FirstOrDefault();
            }
        }

        public IEnumerable<Event> GetOrdersEvents(Guid id)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                return _unitOfWork.context.Set<Event>().Where(e => e.OrderId == id).ToList();
            }
        }

        public void Update(Event entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                try
                {
                    var local = _unitOfWork.context.Set<Event>()
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
                    var clientValues = (Event)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry != null)
                    {
                        var databaseValues = (Event)databaseEntry.ToObject();

                        UpdateException updateException = new UpdateException(databaseValues);
                        throw updateException;
                    }
                }
            }
        }
    }
}
