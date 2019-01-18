using DataModels.Exceptions;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Abstraction;
using RepositoryLayer.ContextFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Repository
{
    public class DocumentRepository : IRepository<Document, Guid>
    {
        private IUnitOfWork _unitOfWork;
        private IDatabaseContextFactory _databaseContextFactory;

        public DocumentRepository()
        {
            _databaseContextFactory = new DatabaseContextFactory();
        }

        public void Add(Document entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                _unitOfWork.context.Set<Document>().Add(entity);
                _unitOfWork.Commit();
            }
        }

        public void Delete(Document entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                Document existing = _unitOfWork.context.Set<Document>().Find(entity);
                if (existing != null)
                {
                    _unitOfWork.context.Set<Document>().Remove(entity);
                    _unitOfWork.Commit();
                }
            }
        }

        public Document Get(Guid key)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                return _unitOfWork.context.Set<Document>().Where(d => d.DocumentId == key).FirstOrDefault();
            }
        }

        public void Update(Document entity)
        {
            using (_unitOfWork = new UnitOfWork(_databaseContextFactory.Create()))
            {
                try
                {
                    var local = _unitOfWork.context.Set<Document>()
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
                    var clientValues = (Document)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry != null)
                    {
                        var databaseValues = (Document)databaseEntry.ToObject();

                        UpdateException updateException = new UpdateException(databaseValues);
                        throw updateException;
                    }
                }
            }
        }
    }
}
